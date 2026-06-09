using Sequence.Web.ViewModels.Watchlist;

namespace Sequence.Web.Services.Interfaces;

public interface IWatchlistService
{
    Task<List<WatchlistItemViewModel>> GetWatchlistAsync(string userId);
    Task<int> AddToWatchlistAsync(WatchlistAddViewModel model, string userId);
    Task<bool> RemoveFromWatchlistAsync(int itemId, string userId);
    Task<MarkAsWatchedViewModel?> GetMarkAsWatchedFormAsync(int itemId, string userId);
    Task<int> MarkAsWatchedAsync(MarkAsWatchedViewModel model, string userId);
}