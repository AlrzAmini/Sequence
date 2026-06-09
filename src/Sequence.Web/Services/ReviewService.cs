
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Sequence.Web.Data;
using Sequence.Web.Domain.Entities;
using Sequence.Web.Domain.Enums;
using Sequence.Web.Services.Interfaces;
using Sequence.Web.ViewModels.Reviews;

namespace Sequence.Web.Services;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ReviewService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ReviewDetailViewModel?> GetReviewDetailAsync(int reviewId, string? currentUserId)
    {
        var review = await _db.Reviews
            .Include(r => r.Movie)
            .Include(r => r.User)
            .Include(r => r.Comments)
                .ThenInclude(c => c.User)
            .Include(r => r.Reactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        if (review is null) return null;

        var vm = _mapper.Map<ReviewDetailViewModel>(review);
        vm.CurrentUserIsAuthor = currentUserId == review.UserId;
        vm.CurrentUserReaction = currentUserId is not null
            ? review.Reactions
                .FirstOrDefault(r => r.UserId == currentUserId)?.ReactionType.ToString()
            : null;

        vm.Comments = review.Comments
            .OrderBy(c => c.CreatedAt)
            .Select(c =>
            {
                var cvm = _mapper.Map<CommentViewModel>(c);
                cvm.CurrentUserIsAuthor = c.UserId == currentUserId;
                return cvm;
            }).ToList();

        return vm;
    }

    public async Task<int> CreateReviewAsync(ReviewCreateViewModel model, string userId)
    {
        // Check: user already reviewed this movie?
        var exists = await _db.Reviews
            .AnyAsync(r => r.MovieId == model.MovieId && r.UserId == userId);

        if (exists) return -1; // Signal duplicate

        var review = _mapper.Map<Review>(model);
        review.UserId = userId;
        review.CreatedAt = DateTime.UtcNow;

        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
        return review.Id;
    }

    public async Task<bool> UpdateReviewAsync(int reviewId, ReviewCreateViewModel model, string userId)
    {
        var review = await _db.Reviews
            .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

        if (review is null) return false;

        review.Body = model.Body;
        review.ContainsSpoiler = model.ContainsSpoiler;
        review.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteReviewAsync(int reviewId, string userId)
    {
        var review = await _db.Reviews
            .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

        if (review is null) return false;

        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<(bool success, string message)> ReactToReviewAsync(
        int reviewId, string userId, string reactionType)
    {
        if (!Enum.TryParse<ReactionType>(reactionType, true, out var parsedReaction))
            return (false, "نوع واکنش نامعتبر است");

        var existing = await _db.ReviewReactions
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userId);

        if (existing is not null)
        {
            if (existing.ReactionType == parsedReaction)
            {
                // Toggle off
                _db.ReviewReactions.Remove(existing);
                await _db.SaveChangesAsync();
                return (true, "واکنش حذف شد");
            }
            else
            {
                // Change reaction
                existing.ReactionType = parsedReaction;
                await _db.SaveChangesAsync();
                return (true, "واکنش تغییر کرد");
            }
        }

        var reaction = new ReviewReaction
        {
            ReviewId = reviewId,
            UserId = userId,
            ReactionType = parsedReaction,
            CreatedAt = DateTime.UtcNow
        };

        _db.ReviewReactions.Add(reaction);
        await _db.SaveChangesAsync();
        return (true, "واکنش ثبت شد");
    }

    public async Task<bool> AddCommentAsync(int reviewId, string body, string userId)
    {
        var reviewExists = await _db.Reviews.AnyAsync(r => r.Id == reviewId);
        if (!reviewExists) return false;

        var comment = new Comment
        {
            ReviewId = reviewId,
            UserId = userId,
            Body = body,
            CreatedAt = DateTime.UtcNow
        };

        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCommentAsync(int commentId, string userId)
    {
        var comment = await _db.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId && c.UserId == userId);

        if (comment is null) return false;

        _db.Comments.Remove(comment);
        await _db.SaveChangesAsync();
        return true;
    }
}