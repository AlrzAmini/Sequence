using Sequence.Web.Domain.Enums;

namespace Sequence.Web.Domain.Entities;

public class WatchlistItem
{
    public int Id { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? PosterUrl { get; set; }
    public int? ReleaseYear { get; set; }
    public Genre Genre { get; set; }
    public string? Notes { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // FK
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
}