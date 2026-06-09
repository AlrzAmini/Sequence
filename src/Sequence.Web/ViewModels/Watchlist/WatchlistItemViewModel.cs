using Sequence.Web.Domain.Enums;

namespace Sequence.Web.ViewModels.Watchlist;

public class WatchlistItemViewModel
{
    public int Id { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? PosterUrl { get; set; }
    public int? ReleaseYear { get; set; }
    public Genre Genre { get; set; }
    public string GenreDisplay { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime AddedAt { get; set; }
}