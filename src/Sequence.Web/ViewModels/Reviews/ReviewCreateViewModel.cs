using System.ComponentModel.DataAnnotations;

namespace Sequence.Web.ViewModels.Reviews;

public class ReviewCreateViewModel
{
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "متن نقد الزامی است")]
    [MinLength(10, ErrorMessage = "نقد باید حداقل ۱۰ کاراکتر باشد")]
    [MaxLength(3000, ErrorMessage = "نقد نمی‌تواند بیش از ۳۰۰۰ کاراکتر باشد")]
    public string Body { get; set; } = string.Empty;

    public bool ContainsSpoiler { get; set; }
}