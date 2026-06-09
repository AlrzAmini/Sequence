using Sequence.Web.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sequence.Web.ViewModels.Watchlist;

public class WatchlistAddViewModel
{
    [Required(ErrorMessage = "عنوان فیلم الزامی است")]
    [MaxLength(200)]
    public string MovieTitle { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? OriginalTitle { get; set; }

    [MaxLength(500)]
    public string? PosterUrl { get; set; }

    [Range(1888, 2100, ErrorMessage = "سال معتبر نیست")]
    public int? ReleaseYear { get; set; }

    public Genre Genre { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}