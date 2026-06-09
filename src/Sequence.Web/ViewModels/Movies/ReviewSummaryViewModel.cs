namespace Sequence.Web.ViewModels.Movies;

public class ReviewSummaryViewModel
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public bool ContainsSpoiler { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserDisplayName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int CommentCount { get; set; }
    public int AgreeCount { get; set; }
    public int DisagreeCount { get; set; }
    public string? CurrentUserReaction { get; set; }
}