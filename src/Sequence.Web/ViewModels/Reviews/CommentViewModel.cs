namespace Sequence.Web.ViewModels.Reviews;

public class CommentViewModel
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorDisplayName { get; set; } = string.Empty;
    public bool CurrentUserIsAuthor { get; set; }
}