using Sequence.Web.Domain.Enums;

namespace Sequence.Web.ViewModels.Movies;

public class MovieDetailViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? PosterUrl { get; set; }
    public int ReleaseYear { get; set; }
    public Genre Genre { get; set; }
    public string GenreDisplay { get; set; } = string.Empty;
    public int PersonalRating { get; set; }
    public string? ShortReview { get; set; }
    public DateTime WatchedDate { get; set; }
    public string CreatedByDisplayName { get; set; } = string.Empty;
    public string CreatedByUserId { get; set; } = string.Empty;
    public List<ReviewSummaryViewModel> Reviews { get; set; } = new();
    public bool CurrentUserHasReviewed { get; set; }
}