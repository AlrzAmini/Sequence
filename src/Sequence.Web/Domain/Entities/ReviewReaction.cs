using Sequence.Web.Domain.Enums;

namespace Sequence.Web.Domain.Entities;

public class ReviewReaction
{
    public int Id { get; set; }
    public ReactionType ReactionType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // FK
    public int ReviewId { get; set; }
    public Review Review { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
}