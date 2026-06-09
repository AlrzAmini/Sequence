namespace Sequence.Web.ViewModels.Dashboard;

public class RecentReviewViewModel
{
    public int ReviewId { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string? MoviePosterUrl { get; set; }
    public string AuthorDisplayName { get; set; } = string.Empty;
    public string BodyPreview { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}