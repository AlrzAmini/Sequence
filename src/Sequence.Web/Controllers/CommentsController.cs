using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sequence.Web.Services.Interfaces;
using System.Security.Claims;

namespace Sequence.Web.Controllers;

[Authorize]
public class CommentsController : Controller
{
    private readonly IReviewService _reviewService;

    public CommentsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int reviewId, string body)
    {
        if (string.IsNullOrWhiteSpace(body) || body.Length < 2)
        {
            TempData["Error"] = "دیدگاه باید حداقل ۲ کاراکتر باشد";
            return RedirectToAction("Detail", "Reviews", new { id = reviewId });
        }

        await _reviewService.AddCommentAsync(reviewId, body, CurrentUserId);
        TempData["Success"] = "دیدگاه شما ثبت شد";
        return RedirectToAction("Detail", "Reviews", new { id = reviewId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int reviewId)
    {
        await _reviewService.DeleteCommentAsync(id, CurrentUserId);
        TempData["Success"] = "دیدگاه حذف شد";
        return RedirectToAction("Detail", "Reviews", new { id = reviewId });
    }
}