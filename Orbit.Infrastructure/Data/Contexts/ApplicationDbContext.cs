using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

            entity.HasMany(d => d.Followers).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_2"),
                    j =>
                    {
                        j.HasKey("UserId", "FollowerId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("follower");
                        j.HasIndex(new[] { "FollowerId" }, "follower_id");
                        j.IndexerProperty<uint>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<uint>("FollowerId").HasColumnName("follower_id");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_1"),
                    j =>
                    {
                        j.HasKey("UserId", "FollowerId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("follower");
                        j.HasIndex(new[] { "FollowerId" }, "follower_id");
                        j.IndexerProperty<uint>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<uint>("FollowerId").HasColumnName("follower_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<User>())
        {
            if (entry.State == EntityState.Modified || entry.State == EntityState.Added) 
            {
                entry.Entity.UserName = entry.Entity.UserName.Trim();
                entry.Entity.UserEmail = entry.Entity.UserEmail.Trim();
                entry.Entity.UserPassword = entry.Entity.UserPassword.Trim();
                entry.Entity.UserDescription = entry.Entity.UserDescription!.Trim();
                entry.Entity.UserDescription = entry.Entity.UserProfileName!.Trim();
            }
        }
        // Atualmente os espaços em brancos são limpos em scripts
        // após serem enviados, ainda no navegador, porém os
        // scripts podem ser alterados manualmente então um fallback
        // é configurado para remover espaços em branco logo antes das
        // mudanças serem commitados.
        return base.SaveChanges();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
