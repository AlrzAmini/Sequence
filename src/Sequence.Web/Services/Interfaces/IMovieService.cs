using Sequence.Web.ViewModels.Movies;

namespace Sequence.Web.Services.Interfaces;

public interface IMovieService
{
    Task<List<MovieListViewModel>> GetAllMoviesAsync(string? userId = null);
    Task<MovieDetailViewModel?> GetMovieDetailAsync(int id, string? currentUserId);
    Task<int> CreateMovieAsync(MovieCreateViewModel model, string userId);
    Task<bool> UpdateMovieAsync(int id, MovieEditViewModel model, string userId);
    Task<bool> DeleteMovieAsync(int id, string userId);
    Task<MovieEditViewModel?> GetMovieForEditAsync(int id, string userId);
    Task<List<MovieListViewModel>> GetHighestRatedAsync(int count = 10);
    Task<List<MovieListViewModel>> GetRecentlyWatchedAsync(int count = 10);
    Task<List<MovieListViewModel>> GetMostDiscussedAsync(int count = 10);
}