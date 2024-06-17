using System;
using System.Collections.Generic;
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
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UserEmail, "user_email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UserId, "user_id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UserName, "user_name_UNIQUE").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserDateOfBirth).HasColumnName("user_date_of_birth");
            entity.Property(e => e.UserDescription)
                .HasColumnType("mediumtext")
                .HasColumnName("user_description")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(200)
                .HasColumnName("user_email")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserImageByteType).HasColumnName("user_image_byte_type");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("user_name")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(200)
                .HasColumnName("user_password")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserProfileName)
                .HasMaxLength(200)
                .HasColumnName("user_profile_name")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            entity.HasMany(d => d.FollowedUsers).WithMany(p => p.FollowerUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("FollowedUserId")
                        .HasConstraintName("fk_followed_user"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerUserId")
                        .HasConstraintName("fk_follower_user"),
                    j =>
                    {
                        j.HasKey("FollowerUserId", "FollowedUserId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("follower");
                        j.HasIndex(new[] { "FollowedUserId" }, "fk_follower_user1_idx");
                        j.IndexerProperty<uint>("FollowerUserId").HasColumnName("follower_user_id");
                        j.IndexerProperty<uint>("FollowedUserId").HasColumnName("followed_user_id");
                    });

            entity.HasMany(d => d.FollowerUsers).WithMany(p => p.FollowedUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerUserId")
                        .HasConstraintName("fk_follower_user"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("FollowedUserId")
                        .HasConstraintName("fk_followed_user"),
                    j =>
                    {
                        j.HasKey("FollowerUserId", "FollowedUserId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("follower");
                        j.HasIndex(new[] { "FollowedUserId" }, "fk_follower_user1_idx");
                        j.IndexerProperty<uint>("FollowerUserId").HasColumnName("follower_user_id");
                        j.IndexerProperty<uint>("FollowedUserId").HasColumnName("followed_user_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
