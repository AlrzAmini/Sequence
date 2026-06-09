using Sequence.Web.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sequence.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<WatchlistItem> WatchlistItems => Set<WatchlistItem>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<ReviewReaction> ReviewReactions => Set<ReviewReaction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ── ApplicationUser ──────────────────────────────────────────
        builder.Entity<ApplicationUser>(e =>
        {
            e.Property(u => u.DisplayName)
             .HasMaxLength(100)
             .IsRequired();

            e.Property(u => u.AvatarUrl)
             .HasMaxLength(500);

            e.Property(u => u.Bio)
             .HasMaxLength(500);
        });

        // ── Movie ─────────────────────────────────────────────────────
        builder.Entity<Movie>(e =>
        {
            e.HasKey(m => m.Id);

            e.Property(m => m.Title)
             .HasMaxLength(200)
             .IsRequired();

            e.Property(m => m.OriginalTitle)
             .HasMaxLength(200);

            e.Property(m => m.PosterUrl)
             .HasMaxLength(500);

            e.Property(m => m.ShortReview)
             .HasMaxLength(1000);

            e.HasOne(m => m.CreatedByUser)
             .WithMany(u => u.Movies)
             .HasForeignKey(m => m.CreatedByUserId)
             .OnDelete(DeleteBehavior.Restrict);

            // Seed data friendly index
            e.HasIndex(m => m.CreatedByUserId);
            e.HasIndex(m => m.WatchedDate);
            e.HasIndex(m => m.PersonalRating);
        });

        // ── WatchlistItem ─────────────────────────────────────────────
        builder.Entity<WatchlistItem>(e =>
        {
            e.HasKey(w => w.Id);

            e.Property(w => w.MovieTitle)
             .HasMaxLength(200)
             .IsRequired();

            e.Property(w => w.OriginalTitle)
             .HasMaxLength(200);

            e.Property(w => w.PosterUrl)
             .HasMaxLength(500);

            e.Property(w => w.Notes)
             .HasMaxLength(500);

            e.HasOne(w => w.User)
             .WithMany(u => u.WatchlistItems)
             .HasForeignKey(w => w.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Review ────────────────────────────────────────────────────
        builder.Entity<Review>(e =>
        {
            e.HasKey(r => r.Id);

            e.Property(r => r.Body)
             .HasMaxLength(3000)
             .IsRequired();

            e.HasOne(r => r.Movie)
             .WithMany(m => m.Reviews)
             .HasForeignKey(r => r.MovieId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(r => r.User)
             .WithMany(u => u.Reviews)
             .HasForeignKey(r => r.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            // One review per user per movie
            e.HasIndex(r => new { r.MovieId, r.UserId }).IsUnique();
        });

        // ── Comment ───────────────────────────────────────────────────
        builder.Entity<Comment>(e =>
        {
            e.HasKey(c => c.Id);

            e.Property(c => c.Body)
             .HasMaxLength(500)
             .IsRequired();

            e.HasOne(c => c.Review)
             .WithMany(r => r.Comments)
             .HasForeignKey(c => c.ReviewId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(c => c.User)
             .WithMany(u => u.Comments)
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── ReviewReaction ────────────────────────────────────────────
        builder.Entity<ReviewReaction>(e =>
        {
            e.HasKey(r => r.Id);

            // One reaction per user per review (enforced at DB level)
            e.HasIndex(r => new { r.ReviewId, r.UserId }).IsUnique();

            e.HasOne(r => r.Review)
             .WithMany(rv => rv.Reactions)
             .HasForeignKey(r => r.ReviewId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(r => r.User)
             .WithMany(u => u.Reactions)
             .HasForeignKey(r => r.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Rename Identity tables to Persian-friendly names (optional)
        builder.Entity<ApplicationUser>().ToTable("Users");
    }
}