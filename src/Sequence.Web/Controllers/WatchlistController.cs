using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sequence.Web.Mappings;
using Sequence.Web.Services.Interfaces;
using Sequence.Web.ViewModels.Watchlist;
using System.Security.Claims;

namespace Sequence.Web.Controllers;

[Authorize]
public class WatchlistController : Controller
{
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(IWatchlistService watchlistService)
    {
        _watchlistService = watchlistService;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index()
    {
        var items = await _watchlistService.GetWatchlistAsync(CurrentUserId);
        return View(items);
    }

    public IActionResult Add()
    {
        PopulateGenreDropdown();
        return View(new WatchlistAddViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(WatchlistAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateGenreDropdown();
            return View(model);
        }

        await _watchlistService.AddToWatchlistAsync(model, CurrentUserId);
        TempData["Success"] = "فیلم به لیست تماشا اضافه شد";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int id)
    {
        await _watchlistService.RemoveFromWatchlistAsync(id, CurrentUserId);
        TempData["Success"] = "فیلم از لیست تماشا حذف شد";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> MarkWatched(int id)
    {
        var model = await _watchlistService.GetMarkAsWatchedFormAsync(id, CurrentUserId);
        if (model is null) return NotFound();

        PopulateGenreDropdown();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkWatched(MarkAsWatchedViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateGenreDropdown();
            return View(model);
        }

        var movieId = await _watchlistService.MarkAsWatchedAsync(model, CurrentUserId);
        if (movieId == 0) return NotFound();

        TempData["Success"] = "فیلم به عنوان تماشا شده ثبت شد!";
        return RedirectToAction("Detail", "Movies", new { id = movieId });
    }

    private void PopulateGenreDropdown()
    {
        ViewBag.Genres = GenreHelper.GetAllFarsi()
            .Select(g => new SelectListItem(g.Value, g.Key.ToString()))
            .ToList();
    }
}