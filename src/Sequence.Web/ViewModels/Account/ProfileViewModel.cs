using System.ComponentModel.DataAnnotations;

namespace Sequence.Web.ViewModels.Account;

public class ProfileViewModel
{
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام نمایشی الزامی است")]
    [MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? AvatarUrl { get; set; }

    [MaxLength(500, ErrorMessage = "بیوگرافی نمی‌تواند بیش از ۵۰۰ کاراکتر باشد")]
    public string? Bio { get; set; }

    public DateTime JoinedAt { get; set; }
    public int TotalMoviesWatched { get; set; }
    public double AverageRating { get; set; }
}