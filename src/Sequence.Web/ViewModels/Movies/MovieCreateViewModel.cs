using Sequence.Web.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sequence.Web.ViewModels.Movies
{
    public class MovieCreateViewModel
    {
        [Required(ErrorMessage = "عنوان فیلم الزامی است")]
        [MaxLength(200, ErrorMessage = "عنوان نمی‌تواند بیش از ۲۰۰ کاراکتر باشد")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "عنوان اصلی نمی‌تواند بیش از ۲۰۰ کاراکتر باشد")]
        public string? OriginalTitle { get; set; }

        [MaxLength(500)]
        public string? PosterUrl { get; set; }

        [Required(ErrorMessage = "سال اکران الزامی است")]
        [Range(1888, 2100, ErrorMessage = "سال اکران معتبر نیست")]
        public int ReleaseYear { get; set; } = DateTime.Now.Year;

        [Required(ErrorMessage = "ژانر را انتخاب کنید")]
        public Genre Genre { get; set; }

        [Required(ErrorMessage = "امتیاز شخصی الزامی است")]
        [Range(1, 10, ErrorMessage = "امتیاز باید بین ۱ تا ۱۰ باشد")]
        public int PersonalRating { get; set; }

        [MaxLength(1000, ErrorMessage = "خلاصه نظر نمی‌تواند بیش از ۱۰۰۰ کاراکتر باشد")]
        public string? ShortReview { get; set; }

        [Required(ErrorMessage = "تاریخ تماشا الزامی است")]
        public DateTime WatchedDate { get; set; } = DateTime.Today;
    }
}
