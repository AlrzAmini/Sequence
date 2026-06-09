using Sequence.Web.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sequence.Web.ViewModels.Watchlist;

public class MarkAsWatchedViewModel
{
    public int WatchlistItemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public string? PosterUrl { get; set; }
    public int ReleaseYear { get; set; } = DateTime.Now.Year;
    public Genre Genre { get; set; }

    [Required(ErrorMessage = "امتیاز شخصی الزامی است")]
    [Range(1, 10, ErrorMessage = "امتیاز باید بین ۱ تا ۱۰ باشد")]
    public int PersonalRating { get; set; }

    [MaxLength(1000)]
    public string? ShortReview { get; set; }

    [Required(ErrorMessage = "تاریخ تماشا الزامی است")]
    public DateTime WatchedDate { get; set; } = DateTime.Today;
}