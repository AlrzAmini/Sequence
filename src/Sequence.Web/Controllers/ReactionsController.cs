using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sequence.Web.Services.Interfaces;
using System.Security.Claims;

namespace Sequence.Web.Controllers;

[Authorize]
[Route("api/reactions")]
public class ReactionsController : Controller
{
    private readonly IReviewService _reviewService;

    public ReactionsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost("{reviewId}/{reactionType}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> React(int reviewId, string reactionType)
    {
        var (success, message) = await _reviewService.ReactToReviewAsync(
            reviewId, CurrentUserId, reactionType);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            var counts = await GetReactionCounts(reviewId);
            return Json(new { success, message, counts });
        }

        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction("Detail", "Reviews", new { id = reviewId });
    }

    private async Task<object> GetReactionCounts(int reviewId)
    {
        // Lightweight count query
        var reactions = await _reviewService.GetReviewDetailAsync(reviewId, CurrentUserId);
        return new
        {
            agree = reactions?.AgreeCount ?? 0,
            disagree = reactions?.DisagreeCount ?? 0,
            userReaction = reactions?.CurrentUserReaction
        };
    }
}