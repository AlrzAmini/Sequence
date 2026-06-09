using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Sequence.Web.Data;
using Sequence.Web.Services.Interfaces;
using Sequence.Web.ViewModels.Dashboard;

namespace Sequence.Web.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _db;
    private readonly IMovieService _movieService;
    private readonly IMapper _mapper;

    public DashboardService(ApplicationDbContext db, IMovieService movieService, IMapper mapper)
    {
        _db = db;
        _movieService = movieService;
        _mapper = mapper;
    }

    public async Task<DashboardViewModel> GetDashboardAsync(string userId)
    {
        var totalWatched = await _db.Movies
            .CountAsync(m => m.CreatedByUserId == userId);

        var watchlistCount = await _db.WatchlistItems
            .CountAsync(w => w.UserId == userId);

        var avgRating = await _db.Movies
            .Where(m => m.CreatedByUserId == userId)
            .AverageAsync(m => (double?)m.PersonalRating) ?? 0.0;

        var totalReviews = await _db.Reviews
            .CountAsync(r => r.UserId == userId);

        var recentReviews = await _db.Reviews
            .Include(r => r.Movie)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .Take(5)
            .AsNoTracking()
            .ToListAsync();

        var recentReviewVms = recentReviews.Select(r => new RecentReviewViewModel
        {
            ReviewId = r.Id,
            MovieId = r.MovieId,
            MovieTitle = r.Movie.Title,
            MoviePosterUrl = r.Movie.PosterUrl,
            AuthorDisplayName = r.User.DisplayName,
            BodyPreview = r.Body.Length > 120 ? r.Body[..120] + "..." : r.Body,
            CreatedAt = r.CreatedAt
        }).ToList();

        return new DashboardViewModel
        {
            TotalMoviesWatched = totalWatched,
            MoviesInWatchlist = watchlistCount,
            AveragePersonalRating = Math.Round(avgRating, 1),
            TotalReviews = totalReviews,
            RecentlyWatched = await _movieService.GetRecentlyWatchedAsync(6),
            HighestRated = await _movieService.GetHighestRatedAsync(6),
            MostDiscussed = await _movieService.GetMostDiscussedAsync(6),
            RecentReviews = recentReviewVms
        };
    }
}