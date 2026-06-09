using System.ComponentModel.DataAnnotations;

namespace Sequence.Web.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "نام نمایشی الزامی است")]
    [MaxLength(100, ErrorMessage = "نام نمایشی نمی‌تواند بیش از ۱۰۰ کاراکتر باشد")]
    public string DisplayName { get; set; } = string.Empty;

    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MinLength(6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
    [Compare("Password", ErrorMessage = "رمزهای عبور مطابقت ندارند")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}