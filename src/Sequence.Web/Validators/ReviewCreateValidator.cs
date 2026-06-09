using FluentValidation;
using Sequence.Web.ViewModels.Reviews;

namespace Sequence.Web.Validators;

public class ReviewCreateValidator : AbstractValidator<ReviewCreateViewModel>
{
    public ReviewCreateValidator()
    {
        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("متن نقد الزامی است")
            .MinimumLength(10).WithMessage("نقد باید حداقل ۱۰ کاراکتر باشد")
            .MaximumLength(3000).WithMessage("نقد نمی‌تواند بیش از ۳۰۰۰ کاراکتر باشد");

        RuleFor(x => x.MovieId)
            .GreaterThan(0).WithMessage("فیلم مشخص نشده است");
    }
}