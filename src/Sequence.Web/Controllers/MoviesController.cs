using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sequence.Web.Mappings;
using Sequence.Web.Services.Interfaces;
using Sequence.Web.ViewModels.Movies;
using System.Security.Claims;

namespace Sequence.Web.Controllers;

[Authorize]
public class MoviesController : Controller
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var movies = await _movieService.GetAllMoviesAsync();
        return View(movies);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Detail(int id)
    {
        var movie = await _movieService.GetMovieDetailAsync(id, User.IsAuthenticated() ? CurrentUserId : null);
        if (movie is null) return NotFound();
        return View(movie);
    }

    public IActionResult Create()
    {
        PopulateGenreDropdown();
        return View(new MovieCreateViewModel { WatchedDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MovieCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateGenreDropdown();
            return View(model);
        }

        var movieId = await _movieService.CreateMovieAsync(model, CurrentUserId);
        TempData["Success"] = "فیلم با موفقیت اضافه شد";
        return RedirectToAction(nameof(Detail), new { id = movieId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await _movieService.GetMovieForEditAsync(id, CurrentUserId);
        if (model is null) return Forbid();

        PopulateGenreDropdown();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MovieEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateGenreDropdown();
            return View(model);
        }

        var result = await _movieService.UpdateMovieAsync(id, model, CurrentUserId);
        if (!result) return Forbid();

        TempData["Success"] = "فیلم با موفقیت ویرایش شد";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _movieService.DeleteMovieAsync(id, CurrentUserId);
        if (!result) return Forbid();

        TempData["Success"] = "فیلم با موفقیت حذف شد";
        return RedirectToAction(nameof(Index));
    }

    private void PopulateGenreDropdown()
    {
        var genres = GenreHelper.GetAllFarsi()
            .Select(g => new SelectListItem(g.Value, g.Key.ToString()))
            .ToList();
        ViewBag.Genres = genres;
    }
}

// Extension helper
public static class ClaimsPrincipalExtensions
{
    public static bool IsAuthenticated(this System.Security.Claims.ClaimsPrincipal principal)
        => principal.Identity?.IsAuthenticated ?? false;
}