using FluentValidation;
using Sequence.Web.ViewModels.Movies;

namespace Sequence.Web.Validators;

public class MovieCreateValidator : AbstractValidator<MovieCreateViewModel>
{
    public MovieCreateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان فیلم الزامی است")
            .MaximumLength(200).WithMessage("عنوان نمی‌تواند بیش از ۲۰۰ کاراکتر باشد");

        RuleFor(x => x.ReleaseYear)
            .InclusiveBetween(1888, DateTime.Now.Year + 2)
            .WithMessage($"سال اکران باید بین ۱۸۸۸ و {DateTime.Now.Year + 2} باشد");

        RuleFor(x => x.PersonalRating)
            .InclusiveBetween(1, 10)
            .WithMessage("امتیاز باید بین ۱ تا ۱۰ باشد");

        RuleFor(x => x.WatchedDate)
            .NotEmpty().WithMessage("تاریخ تماشا الزامی است")
            .LessThanOrEqualTo(DateTime.Today.AddDays(1))
            .WithMessage("تاریخ تماشا نمی‌تواند در آینده باشد");

        RuleFor(x => x.ShortReview)
            .MaximumLength(1000).WithMessage("خلاصه نظر نمی‌تواند بیش از ۱۰۰۰ کاراکتر باشد")
            .When(x => x.ShortReview is not null);

        RuleFor(x => x.PosterUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("آدرس پوستر معتبر نیست")
            .When(x => !string.IsNullOrEmpty(x.PosterUrl));
    }
}