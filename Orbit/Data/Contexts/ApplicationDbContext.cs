using Laraue.EfCoreTriggers.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Orbit.Models;

namespace Orbit.Data.Contexts;

public partial class ApplicationDbContext : DbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    // Constructor for the ApplicationDbContext, passing DbContextOptions to the base class
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for various models
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Like> Likes { get; set; }
    public virtual DbSet<UserPreference> UserPreferences { get; set; }
    public virtual DbSet<PostPreference> PostPreferences { get; set; }

    // Method to configure the model using Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set the collation for the database
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        // Configure the Like entity
        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PRIMARY");
            entity.Property(e => e.LikeId).HasColumnName("like_id");
            entity.ToTable("like");

            entity.HasIndex(e => e.UserId, "like_ibfk_1");
            entity.HasIndex(e => e.PostId, "like_ibfk_2");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            // Define relationships between entities
            entity.HasOne(d => d.Post).WithMany()
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("like_ibfk_2");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("like_ibfk_1");
            
            entity.HasOne(d => d.Post).WithMany(p => p.Likes);
        });

        // Configure the Post entity
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity.ToTable("post");

            entity.HasIndex(e => e.UserId, "post_ibfk_1");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.PostContent).HasColumnName("post_content");
            entity.Property(e => e.PostDate)
                .HasColumnType("datetime")
                .HasColumnName("post_date")
                .HasDefaultValueSql("(NOW())");
            entity.Property(e => e.PostImageByteType).HasColumnName("post_image_byte_type").HasColumnType("LONGBLOB");
            entity.Property(e => e.PostLikes).HasColumnName("post_likes");
            entity.Property(e => e.PostVideoByteType).HasColumnName("post_video_byte_type").HasColumnType("LONGBLOB");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("post_ibfk_1");
            
            entity.HasMany(d => d.PostPreferences).WithOne(p => p.Post);
            entity.HasMany(d => d.Likes).WithOne(p => p.Post);
        });

        // Configure the User entity
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
            entity.Property(e => e.UserProfileBannerImageByteType).HasColumnName("user_profile_banner_image_byte_type").HasColumnType("LONGBLOB");
            entity.Property(e => e.UserDescription)
                .HasColumnType("mediumtext")
                .HasColumnName("user_description")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserEmail)
                .HasColumnName("user_email")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserProfileImageByteType).HasColumnName("user_profile_image_byte_type").HasColumnType("LONGBLOB");
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
            entity.Property(e => e.UserCurriculumPDFByteType)
                .HasColumnType("LONGBLOB")
                .HasColumnName("user_pdf_curriculum_byte_type");

            // Configure many-to-many relationship for Followers
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
                    
            // Configure one-to-many relationship for UserPreferences
            entity.HasMany(d => d.UserPreferences).WithOne(p => p.User);
        });

        // Configure the UserPreference entity
        modelBuilder.Entity<UserPreference>(entity =>
        {
            entity.HasKey(e => e.PreferenceId).HasName("PRIMARY");

            entity.ToTable("user_preference");

            entity.HasIndex(e => e.UserId, "user_preference_ibfk_1");

            entity.Property(e => e.PreferenceId).HasColumnName("preference_id");
            entity.Property(e => e.PreferenceName)
                .HasMaxLength(255)
                .HasColumnName("preference_name");

            entity.HasOne(d => d.User).WithMany(p => p.UserPreferences)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_preference_ibfk_1");
        });

        // Configure the PostPreference entity
        modelBuilder.Entity<PostPreference>(entity =>
        {
            entity.HasKey(e => e.PreferenceId).HasName("PRIMARY");

            entity.ToTable("post_preference");

            entity.HasIndex(e => e.PostId, "post_preference_ibfk_1");

            entity.Property(e => e.PreferenceId).HasColumnName("preference_id");
            entity.Property(e => e.PreferenceName)
                .HasMaxLength(255)
                .HasColumnName("preference_name");

            entity.HasOne(d => d.Post).WithMany(p => p.PostPreferences)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("post_preference_ibfk_1");
        });

        // Call additional configuration in partial method
        OnModelCreatingPartial(modelBuilder);
    }

    // Partial method for further configuration (optional to override)
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
