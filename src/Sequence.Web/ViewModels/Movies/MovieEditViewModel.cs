using Sequence.Web.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sequence.Web.ViewModels.Movies;

public class MovieEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "عنوان فیلم الزامی است")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? OriginalTitle { get; set; }

    [MaxLength(500)]
    public string? PosterUrl { get; set; }

    [Required(ErrorMessage = "سال اکران الزامی است")]
    [Range(1888, 2100, ErrorMessage = "سال اکران معتبر نیست")]
    public int ReleaseYear { get; set; }

    public Genre Genre { get; set; }

    [Range(1, 10, ErrorMessage = "امتیاز باید بین ۱ تا ۱۰ باشد")]
    public int PersonalRating { get; set; }

    [MaxLength(1000)]
    public string? ShortReview { get; set; }

    public DateTime WatchedDate { get; set; }
}