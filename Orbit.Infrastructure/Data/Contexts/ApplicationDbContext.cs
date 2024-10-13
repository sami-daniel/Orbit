using Microsoft.EntityFrameworkCore;
using Orbit.Domain.Entities;

namespace Orbit.Infrastructure.Data.Contexts;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Like>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("likes");

            entity.HasIndex(e => e.UserId, "like_ibfk_1");

            entity.HasIndex(e => e.PostId, "like_ibfk_2");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany()
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("like_ibfk_2");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("like_ibfk_1");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity.ToTable("post");

            entity.HasIndex(e => e.UserId, "post_ibfk_1");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.PostContent).HasColumnName("post_content");
            entity.Property(e => e.PostDate)
                .HasColumnType("datetime")
                .HasColumnName("post_date");
            entity.Property(e => e.PostImageByteType).HasColumnName("post_image_byte_type");
            entity.Property(e => e.PostLikes).HasColumnName("post_likes");
            entity.Property(e => e.PostVideoByteType).HasColumnName("post_video_byte_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("post_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity
                .ToTable("user")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.UserEmail, "user_email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UserName, "user_name_UNIQUE").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IsPrivateProfile)
                .HasColumnType("bit(1)")
                .HasColumnName("is_private_profile");
            entity.Property(e => e.UserProfileBannerImageByteType).HasColumnName("user_profile_banner_image_byte_type");
            entity.Property(e => e.UserDescription)
                .HasColumnType("mediumtext")
                .HasColumnName("user_description")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserEmail)
                .HasColumnName("user_email")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserProfileImageByteType).HasColumnName("user_profile_image_byte_type");
            entity.Property(e => e.UserName)
                .HasColumnName("user_name")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .HasColumnName("user_password")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserProfileName)
                .HasMaxLength(255)
                .HasColumnName("user_profile_name");

            entity.HasMany(d => d.Followers).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .HasConstraintName("follower_ibfk_1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("follower_ibfk_2"),
                    j =>
                    {
                        j.HasKey("UserId", "FollowerId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j
                            .ToTable("follower")
                            .HasCharSet("utf8mb3")
                            .UseCollation("utf8mb3_general_ci");
                        j.HasIndex(new[] { "FollowerId" }, "follower_id");
                        j.IndexerProperty<uint>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<uint>("FollowerId").HasColumnName("follower_id");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("follower_ibfk_2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .HasConstraintName("follower_ibfk_1"),
                    j =>
                    {
                        j.HasKey("UserId", "FollowerId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j
                            .ToTable("follower")
                            .HasCharSet("utf8mb3")
                            .UseCollation("utf8mb3_general_ci");
                        j.HasIndex(new[] { "FollowerId" }, "follower_id");
                        j.IndexerProperty<uint>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<uint>("FollowerId").HasColumnName("follower_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
