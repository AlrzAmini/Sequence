using Sequence.Web.Domain.Enums;

namespace Sequence.Web.ViewModels.Movies;

public class MovieListViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? PosterUrl { get; set; }
    public int ReleaseYear { get; set; }
    public Genre Genre { get; set; }
    public string GenreDisplay { get; set; } = string.Empty;
    public int PersonalRating { get; set; }
    public DateTime WatchedDate { get; set; }
    public int ReviewCount { get; set; }
    public bool IsOwner { get; set; }
}