namespace Sequence.Web.ViewModels.Reviews;

public class ReviewDetailViewModel
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string? MoviePosterUrl { get; set; }
    public string Body { get; set; } = string.Empty;
    public bool ContainsSpoiler { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorDisplayName { get; set; } = string.Empty;
    public int AgreeCount { get; set; }
    public int DisagreeCount { get; set; }
    public string? CurrentUserReaction { get; set; }
    public bool CurrentUserIsAuthor { get; set; }
    public List<CommentViewModel> Comments { get; set; } = new();
}