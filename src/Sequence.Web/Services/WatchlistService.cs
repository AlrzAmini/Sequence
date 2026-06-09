
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Sequence.Web.Data;
using Sequence.Web.Domain.Entities;
using Sequence.Web.Services.Interfaces;
using Sequence.Web.ViewModels.Watchlist;

namespace Sequence.Web.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public WatchlistService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<WatchlistItemViewModel>> GetWatchlistAsync(string userId)
        {
            var items = await _db.WatchlistItems
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.AddedAt)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<WatchlistItemViewModel>>(items);
        }

        public async Task<int> AddToWatchlistAsync(WatchlistAddViewModel model, string userId)
        {
            var item = _mapper.Map<WatchlistItem>(model);
            item.UserId = userId;
            item.AddedAt = DateTime.UtcNow;

            _db.WatchlistItems.Add(item);
            await _db.SaveChangesAsync();
            return item.Id;
        }

        public async Task<bool> RemoveFromWatchlistAsync(int itemId, string userId)
        {
            var item = await _db.WatchlistItems
                .FirstOrDefaultAsync(w => w.Id == itemId && w.UserId == userId);

            if (item is null) return false;

            _db.WatchlistItems.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<MarkAsWatchedViewModel?> GetMarkAsWatchedFormAsync(int itemId, string userId)
        {
            var item = await _db.WatchlistItems
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == itemId && w.UserId == userId);

            if (item is null) return null;

            return new MarkAsWatchedViewModel
            {
                WatchlistItemId = item.Id,
                Title = item.MovieTitle,
                OriginalTitle = item.OriginalTitle,
                PosterUrl = item.PosterUrl,
                ReleaseYear = item.ReleaseYear ?? DateTime.Now.Year,
                Genre = item.Genre,
                WatchedDate = DateTime.Today
            };
        }

        public async Task<int> MarkAsWatchedAsync(MarkAsWatchedViewModel model, string userId)
        {
            // This is a business transaction: remove from watchlist + create movie
            using var transaction = await _db.Database.BeginTransactionAsync();

            var watchlistItem = await _db.WatchlistItems
                .FirstOrDefaultAsync(w => w.Id == model.WatchlistItemId && w.UserId == userId);

            if (watchlistItem is null) return 0;

            var movie = new Movie
            {
                Title = model.Title,
                OriginalTitle = model.OriginalTitle,
                PosterUrl = model.PosterUrl,
                ReleaseYear = model.ReleaseYear,
                Genre = model.Genre,
                PersonalRating = model.PersonalRating,
                ShortReview = model.ShortReview,
                WatchedDate = model.WatchedDate,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _db.Movies.Add(movie);
            _db.WatchlistItems.Remove(watchlistItem);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return movie.Id;
        }
    }
}
