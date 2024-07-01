using Microsoft.EntityFrameworkCore;
using Orbit.Domain.Entities;

namespace Orbit.Infrastructure.Data.Contexts;

public partial class ApplicationDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");
        _ = modelBuilder.Entity<User>(entity =>
        {
            _ = entity.HasKey(e => e.UserId).HasName("PRIMARY");

            _ = entity.ToTable("user");

            _ = entity.HasIndex(e => e.UserEmail, "user_email_UNIQUE").IsUnique();

            _ = entity.HasIndex(e => e.UserId, "user_id_UNIQUE").IsUnique();

            _ = entity.HasIndex(e => e.UserName, "user_name_UNIQUE").IsUnique();

            _ = entity.Property(e => e.UserId).HasColumnName("user_id");
            _ = entity.Property(e => e.UserDescription)
                .HasColumnType("mediumtext")
                .HasColumnName("user_description")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            _ = entity.Property(e => e.UserEmail)
                .HasMaxLength(200)
                .HasColumnName("user_email")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            _ = entity.Property(e => e.UserImageByteType).HasColumnName("user_image_byte_type");
            _ = entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("user_name")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            _ = entity.Property(e => e.UserPassword)
                .HasMaxLength(200)
                .HasColumnName("user_password")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            _ = entity.Property(e => e.UserProfileName)
                .HasMaxLength(200)
                .HasColumnName("user_profile_name")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            _ = entity.HasMany(d => d.Followers).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("follower_ibfk_2"),
                    j =>
                    {
                        _ = j.HasKey("UserId", "FollowerId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        _ = j.ToTable("follower");
                        _ = j.HasIndex(new[] { "FollowerId" }, "follower_id");
                        _ = j.IndexerProperty<uint>("UserId").HasColumnName("user_id");
                        _ = j.IndexerProperty<uint>("FollowerId").HasColumnName("follower_id");
                    });

            _ = entity.HasMany(d => d.Users).WithMany(p => p.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("follower_ibfk_2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_1"),
                    j =>
                    {
                        _ = j.HasKey("UserId", "FollowerId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        _ = j.ToTable("follower");
                        _ = j.HasIndex(new[] { "FollowerId" }, "follower_id");
                        _ = j.IndexerProperty<uint>("UserId").HasColumnName("user_id");
                        _ = j.IndexerProperty<uint>("FollowerId").HasColumnName("follower_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public override int SaveChanges()
    {
        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> entry in ChangeTracker.Entries<User>())
        {
            if (entry.State is EntityState.Modified or EntityState.Added)
            {
                entry.Entity.UserName = entry.Entity.UserName.Trim();
                entry.Entity.UserEmail = entry.Entity.UserEmail.Trim();
                entry.Entity.UserPassword = entry.Entity.UserPassword.Trim();
                entry.Entity.UserProfileName = entry.Entity.UserProfileName.Trim();
                entry.Entity.UserDescription = entry.Entity.UserDescription?.Trim();
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
