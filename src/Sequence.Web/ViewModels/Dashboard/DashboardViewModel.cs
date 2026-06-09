using Sequence.Web.ViewModels.Movies;

namespace Sequence.Web.ViewModels.Dashboard;

public class DashboardViewModel
{
    public int TotalMoviesWatched { get; set; }
    public int MoviesInWatchlist { get; set; }
    public double AveragePersonalRating { get; set; }
    public int TotalReviews { get; set; }

    public List<MovieListViewModel> RecentlyWatched { get; set; } = new();
    public List<MovieListViewModel> HighestRated { get; set; } = new();
    public List<MovieListViewModel> MostDiscussed { get; set; } = new();
    public List<RecentReviewViewModel> RecentReviews { get; set; } = new();
}