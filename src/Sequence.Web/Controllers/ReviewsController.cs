using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sequence.Web.Services.Interfaces;
using Sequence.Web.ViewModels.Reviews;
using System.Security.Claims;

namespace Sequence.Web.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [AllowAnonymous]
    public async Task<IActionResult> Detail(int id)
    {
        var vm = await _reviewService.GetReviewDetailAsync(
            id,
            User.IsAuthenticated() ? CurrentUserId : null);

        if (vm is null) return NotFound();
        return View(vm);
    }

    public IActionResult Create(int movieId, string movieTitle)
    {
        return View(new ReviewCreateViewModel
        {
            MovieId = movieId,
            MovieTitle = movieTitle
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReviewCreateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var reviewId = await _reviewService.CreateReviewAsync(model, CurrentUserId);

        if (reviewId == -1)
        {
            ModelState.AddModelError("", "شما قبلاً برای این فیلم نقد نوشته‌اید");
            return View(model);
        }

        TempData["Success"] = "نقد شما با موفقیت ثبت شد";
        return RedirectToAction(nameof(Detail), new { id = reviewId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var review = await _reviewService.GetReviewDetailAsync(id, CurrentUserId);
        if (review is null || !review.CurrentUserIsAuthor) return Forbid();

        return View(new ReviewCreateViewModel
        {
            MovieId = review.MovieId,
            MovieTitle = review.MovieTitle,
            Body = review.Body,
            ContainsSpoiler = review.ContainsSpoiler
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReviewCreateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _reviewService.UpdateReviewAsync(id, model, CurrentUserId);
        if (!result) return Forbid();

        TempData["Success"] = "نقد با موفقیت ویرایش شد";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int movieId)
    {
        await _reviewService.DeleteReviewAsync(id, CurrentUserId);
        TempData["Success"] = "نقد حذف شد";
        return RedirectToAction("Detail", "Movies", new { id = movieId });
    }
}