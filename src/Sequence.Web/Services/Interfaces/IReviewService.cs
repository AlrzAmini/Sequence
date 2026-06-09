using Sequence.Web.ViewModels.Reviews;

namespace Sequence.Web.Services.Interfaces;

public interface IReviewService
{
    Task<ReviewDetailViewModel?> GetReviewDetailAsync(int reviewId, string? currentUserId);
    Task<int> CreateReviewAsync(ReviewCreateViewModel model, string userId);
    Task<bool> UpdateReviewAsync(int reviewId, ReviewCreateViewModel model, string userId);
    Task<bool> DeleteReviewAsync(int reviewId, string userId);
    Task<(bool success, string message)> ReactToReviewAsync(int reviewId, string userId, string reactionType);
    Task<bool> AddCommentAsync(int reviewId, string body, string userId);
    Task<bool> DeleteCommentAsync(int commentId, string userId);
}