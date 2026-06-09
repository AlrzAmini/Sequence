using Mapster;
using Sequence.Web.Domain.Entities;
using Sequence.Web.Domain.Enums;
using Sequence.Web.ViewModels.Account;
using Sequence.Web.ViewModels.Movies;
using Sequence.Web.ViewModels.Reviews;
using Sequence.Web.ViewModels.Watchlist;

namespace Sequence.Web.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<Movie, MovieListViewModel>()
            .Map(d => d.GenreDisplay,
                s => GenreHelper.ToFarsi(s.Genre))
            .Map(d => d.ReviewCount,
                s => s.Reviews.Count);

        config.NewConfig<Movie, MovieDetailViewModel>()
            .Map(d => d.GenreDisplay,
                s => GenreHelper.ToFarsi(s.Genre))
            .Map(d => d.CreatedByDisplayName,
                s => s.CreatedByUser.DisplayName);

        config.NewConfig<WatchlistItem, WatchlistItemViewModel>()
            .Map(d => d.GenreDisplay,
                s => GenreHelper.ToFarsi(s.Genre));

        config.NewConfig<Review, ReviewDetailViewModel>()
            .Map(d => d.MovieTitle,
                s => s.Movie.Title)
            .Map(d => d.MoviePosterUrl,
                s => s.Movie.PosterUrl)
            .Map(d => d.AuthorDisplayName,
                s => s.User.DisplayName)
            .Map(d => d.AuthorId,
                s => s.UserId)
            .Map(d => d.AgreeCount,
                s => s.Reactions.Count(r => r.ReactionType == ReactionType.Agree))
            .Map(d => d.DisagreeCount,
                s => s.Reactions.Count(r => r.ReactionType == ReactionType.Disagree));

        config.NewConfig<Comment, CommentViewModel>()
            .Map(d => d.AuthorDisplayName,
                s => s.User.DisplayName)
            .Map(d => d.AuthorId,
                s => s.UserId);

        config.NewConfig<ApplicationUser, ProfileViewModel>()
            .Map(d => d.UserId,
                s => s.Id);
    }
}