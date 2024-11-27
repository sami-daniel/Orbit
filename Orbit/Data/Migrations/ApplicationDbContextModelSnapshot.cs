﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orbit.Data.Contexts;

#nullable disable

namespace Orbit.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Follower", b =>
                {
                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned")
                        .HasColumnName("user_id");

                    b.Property<uint>("FollowerId")
                        .HasColumnType("int unsigned")
                        .HasColumnName("follower_id");

                    b.HasKey("UserId", "FollowerId")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "FollowerId" }, "follower_id");

                    b.ToTable("follower", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Orbit.Models.Like", b =>
                {
                    b.Property<uint>("PostId")
                        .HasColumnType("int unsigned")
                        .HasColumnName("post_id");

                    b.Property<uint?>("UserId")
                        .HasColumnType("int unsigned")
                        .HasColumnName("user_id");

                    b.HasIndex(new[] { "UserId" }, "like_ibfk_1");

                    b.HasIndex(new[] { "PostId" }, "like_ibfk_2");

                    b.ToTable("likes", (string)null);
                });

            modelBuilder.Entity("Orbit.Models.Post", b =>
                {
                    b.Property<uint>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned")
                        .HasColumnName("post_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<uint>("PostId"));

                    b.Property<string>("PostContent")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("post_content");

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("datetime")
                        .HasColumnName("post_date");

                    b.Property<byte[]>("PostImageByteType")
                        .HasColumnType("longblob")
                        .HasColumnName("post_image_byte_type");

                    b.Property<uint>("PostLikes")
                        .HasColumnType("int unsigned")
                        .HasColumnName("post_likes");

                    b.Property<byte[]>("PostVideoByteType")
                        .HasColumnType("longblob")
                        .HasColumnName("post_video_byte_type");

                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("PostId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "UserId" }, "post_ibfk_1");

                    b.ToTable("post", (string)null);
                });

            modelBuilder.Entity("Orbit.Models.PostPreference", b =>
                {
                    b.Property<uint>("PreferenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned")
                        .HasColumnName("preference_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<uint>("PreferenceId"));

                    b.Property<uint>("PostId")
                        .HasColumnType("int unsigned");

                    b.Property<string>("PreferenceName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("preference_name");

                    b.HasKey("PreferenceId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "PostId" }, "post_preference_ibfk_1");

                    b.ToTable("post_preference", (string)null);
                });

            modelBuilder.Entity("Orbit.Models.User", b =>
                {
                    b.Property<uint>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned")
                        .HasColumnName("user_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<uint>("UserId"));

                    b.Property<string>("UserDescription")
                        .HasColumnType("mediumtext")
                        .HasColumnName("user_description")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("UserDescription"), "utf8mb4");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("user_email")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("UserEmail"), "utf8mb4");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("user_name")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("UserName"), "utf8mb4");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("user_password")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("UserPassword"), "utf8mb4");

                    b.Property<byte[]>("UserProfileBannerImageByteType")
                        .HasColumnType("longblob")
                        .HasColumnName("user_profile_banner_image_byte_type");

                    b.Property<byte[]>("UserProfileImageByteType")
                        .HasColumnType("longblob")
                        .HasColumnName("user_profile_image_byte_type");

                    b.Property<string>("UserProfileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("user_profile_name");

                    b.HasKey("UserId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "UserEmail" }, "user_email_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "UserName" }, "user_name_UNIQUE")
                        .IsUnique();

                    b.ToTable("user", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Orbit.Models.UserPreference", b =>
                {
                    b.Property<uint>("PreferenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned")
                        .HasColumnName("preference_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<uint>("PreferenceId"));

                    b.Property<string>("PreferenceName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("preference_name");

                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned");

                    b.HasKey("PreferenceId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "UserId" }, "user_preference_ibfk_1");

                    b.ToTable("user_preference", (string)null);
                });

            modelBuilder.Entity("Follower", b =>
                {
                    b.HasOne("Orbit.Models.User", null)
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("follower_ibfk_1");

                    b.HasOne("Orbit.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("follower_ibfk_2");
                });

            modelBuilder.Entity("Orbit.Models.Like", b =>
                {
                    b.HasOne("Orbit.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("like_ibfk_2");

                    b.HasOne("Orbit.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("like_ibfk_1");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Orbit.Models.Post", b =>
                {
                    b.HasOne("Orbit.Models.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("post_ibfk_1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Orbit.Models.PostPreference", b =>
                {
                    b.HasOne("Orbit.Models.Post", "Post")
                        .WithMany("PostPreferences")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("post_preference_ibfk_1");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Orbit.Models.UserPreference", b =>
                {
                    b.HasOne("Orbit.Models.User", "User")
                        .WithMany("UserPreferences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("user_preference_ibfk_1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Orbit.Models.Post", b =>
                {
                    b.Navigation("PostPreferences");
                });

            modelBuilder.Entity("Orbit.Models.User", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("UserPreferences");
                });
#pragma warning restore 612, 618
        }
    }
}
