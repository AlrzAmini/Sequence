using FluentValidation;
using Sequence.Web.ViewModels.Reviews;

namespace Sequence.Web.Validators;

public class CommentValidator : AbstractValidator<CommentViewModel>
{
    public CommentValidator()
    {
        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("متن دیدگاه الزامی است")
            .MinimumLength(2).WithMessage("دیدگاه باید حداقل ۲ کاراکتر باشد")
            .MaximumLength(500).WithMessage("دیدگاه نمی‌تواند بیش از ۵۰۰ کاراکتر باشد");
    }
}