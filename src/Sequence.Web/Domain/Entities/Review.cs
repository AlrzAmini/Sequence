namespace Sequence.Web.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public bool ContainsSpoiler { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // FK
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    // Navigation
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<ReviewReaction> Reactions { get; set; } = new List<ReviewReaction>();
}