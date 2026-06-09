
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Sequence.Web.Data;
using Sequence.Web.Domain.Entities;
using Sequence.Web.Domain.Enums;
using Sequence.Web.Services.Interfaces;
using Sequence.Web.ViewModels.Movies;

namespace Sequence.Web.Services;

public class MovieService : IMovieService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public MovieService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<MovieListViewModel>> GetAllMoviesAsync(string? userId = null)
    {
        var query = _db.Movies
            .Include(m => m.Reviews)
            .Include(m => m.CreatedByUser)
            .AsNoTracking();

        if (userId is not null)
            query = query.Where(m => m.CreatedByUserId == userId);

        var movies = await query
            .OrderByDescending(m => m.WatchedDate)
            .ToListAsync();

        return movies.Select(m =>
        {
            var vm = _mapper.Map<MovieListViewModel>(m);
            vm.IsOwner = m.CreatedByUserId == userId;
            return vm;
        }).ToList();
    }

    public async Task<MovieDetailViewModel?> GetMovieDetailAsync(int id, string? currentUserId)
    {
        var movie = await _db.Movies
            .Include(m => m.CreatedByUser)
            .Include(m => m.Reviews)
                .ThenInclude(r => r.User)
            .Include(m => m.Reviews)
                .ThenInclude(r => r.Comments)
            .Include(m => m.Reviews)
                .ThenInclude(r => r.Reactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie is null) return null;

        var vm = _mapper.Map<MovieDetailViewModel>(movie);

        vm.Reviews = movie.Reviews.Select(r => new ReviewSummaryViewModel
        {
            Id = r.Id,
            Body = r.Body,
            ContainsSpoiler = r.ContainsSpoiler,
            CreatedAt = r.CreatedAt,
            UserDisplayName = r.User.DisplayName,
            UserId = r.UserId,
            CommentCount = r.Comments.Count,
            AgreeCount = r.Reactions.Count(rx => rx.ReactionType == ReactionType.Agree),
            DisagreeCount = r.Reactions.Count(rx => rx.ReactionType == ReactionType.Disagree),
            CurrentUserReaction = currentUserId is not null
                ? r.Reactions.FirstOrDefault(rx => rx.UserId == currentUserId)?.ReactionType.ToString()
                : null
        }).ToList();

        vm.CurrentUserHasReviewed = currentUserId is not null &&
            movie.Reviews.Any(r => r.UserId == currentUserId);

        return vm;
    }

    public async Task<int> CreateMovieAsync(MovieCreateViewModel model, string userId)
    {
        var movie = _mapper.Map<Movie>(model);
        movie.CreatedByUserId = userId;
        movie.CreatedAt = DateTime.UtcNow;

        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();
        return movie.Id;
    }

    public async Task<bool> UpdateMovieAsync(int id, MovieEditViewModel model, string userId)
    {
        var movie = await _db.Movies
            .FirstOrDefaultAsync(m => m.Id == id && m.CreatedByUserId == userId);

        if (movie is null) return false;

        _mapper.Map(model, movie);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMovieAsync(int id, string userId)
    {
        var movie = await _db.Movies
            .FirstOrDefaultAsync(m => m.Id == id && m.CreatedByUserId == userId);

        if (movie is null) return false;

        _db.Movies.Remove(movie);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<MovieEditViewModel?> GetMovieForEditAsync(int id, string userId)
    {
        var movie = await _db.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id && m.CreatedByUserId == userId);

        return movie is null ? null : _mapper.Map<MovieEditViewModel>(movie);
    }

    public async Task<List<MovieListViewModel>> GetHighestRatedAsync(int count = 10)
    {
        var movies = await _db.Movies
            .Include(m => m.Reviews)
            .Include(m => m.CreatedByUser)
            .OrderByDescending(m => m.PersonalRating)
            .ThenByDescending(m => m.WatchedDate)
            .Take(count)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<MovieListViewModel>>(movies);
    }

    public async Task<List<MovieListViewModel>> GetRecentlyWatchedAsync(int count = 10)
    {
        var movies = await _db.Movies
            .Include(m => m.Reviews)
            .Include(m => m.CreatedByUser)
            .OrderByDescending(m => m.WatchedDate)
            .Take(count)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<MovieListViewModel>>(movies);
    }

    public async Task<List<MovieListViewModel>> GetMostDiscussedAsync(int count = 10)
    {
        var movies = await _db.Movies
            .Include(m => m.Reviews)
                .ThenInclude(r => r.Comments)
            .Include(m => m.CreatedByUser)
            .AsNoTracking()
            .ToListAsync();

        return movies
            .OrderByDescending(m => m.Reviews.Sum(r => r.Comments.Count))
            .Take(count)
            .Select(m => _mapper.Map<MovieListViewModel>(m))
            .ToList();
    }
}