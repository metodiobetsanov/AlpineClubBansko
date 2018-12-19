﻿// <auto-generated />
using System;
using AlpineClubBansko.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlpineClubBansko.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Album", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("Rating");

                    b.Property<string>("RouteId");

                    b.Property<string>("StoryId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("RouteId")
                        .IsUnique()
                        .HasFilter("[RouteId] IS NOT NULL");

                    b.HasIndex("StoryId")
                        .IsUnique()
                        .HasFilter("[StoryId] IS NOT NULL");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Event", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlbumId");

                    b.Property<string>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("Rating");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Level", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId")
                        .IsUnique()
                        .HasFilter("[AuthorId] IS NOT NULL");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Location", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("RouteId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("RouteId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Photo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlbumId");

                    b.Property<string>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LocationUrl");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Photographer");

                    b.Property<int>("Rating");

                    b.Property<string>("ThumbnailUrl");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Route", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("Rating");

                    b.Property<string>("StoryId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("StoryId")
                        .IsUnique()
                        .HasFilter("[StoryId] IS NOT NULL");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Story", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("Rating");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Avatar");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int>("PostCode");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.UsersEvents", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EventId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersEvents");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Album", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.User", "Author")
                        .WithMany("Albums")
                        .HasForeignKey("AuthorId");

                    b.HasOne("AlpineClubBansko.Data.Models.Route", "Route")
                        .WithOne("Album")
                        .HasForeignKey("AlpineClubBansko.Data.Models.Album", "RouteId");

                    b.HasOne("AlpineClubBansko.Data.Models.Story", "Story")
                        .WithOne("Album")
                        .HasForeignKey("AlpineClubBansko.Data.Models.Album", "StoryId");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Event", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.Album", "Album")
                        .WithMany()
                        .HasForeignKey("AlbumId");

                    b.HasOne("AlpineClubBansko.Data.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Level", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.User", "Author")
                        .WithOne("Level")
                        .HasForeignKey("AlpineClubBansko.Data.Models.Level", "AuthorId");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Location", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("AlpineClubBansko.Data.Models.Route")
                        .WithMany("Locations")
                        .HasForeignKey("RouteId");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Photo", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.Album", "Album")
                        .WithMany("Photos")
                        .HasForeignKey("AlbumId");

                    b.HasOne("AlpineClubBansko.Data.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Route", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.User", "Author")
                        .WithMany("Routes")
                        .HasForeignKey("AuthorId");

                    b.HasOne("AlpineClubBansko.Data.Models.Story", "Story")
                        .WithOne("Route")
                        .HasForeignKey("AlpineClubBansko.Data.Models.Route", "StoryId");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.Story", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.User", "Author")
                        .WithMany("Stories")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("AlpineClubBansko.Data.Models.UsersEvents", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.Event", "Event")
                        .WithMany("Participants")
                        .HasForeignKey("EventId");

                    b.HasOne("AlpineClubBansko.Data.Models.User", "User")
                        .WithMany("EventsParticipations")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AlpineClubBansko.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AlpineClubBansko.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
