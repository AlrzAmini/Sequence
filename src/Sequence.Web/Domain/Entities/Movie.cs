using Sequence.Web.Domain.Enums;

namespace Sequence.Web.Domain.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? PosterUrl { get; set; }
    public int ReleaseYear { get; set; }
    public Genre Genre { get; set; }
    public int PersonalRating { get; set; }      // 1 to 10
    public string? ShortReview { get; set; }
    public DateTime WatchedDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // FK
    public string CreatedByUserId { get; set; } = string.Empty;
    public ApplicationUser CreatedByUser { get; set; } = null!;

    // Navigation
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}