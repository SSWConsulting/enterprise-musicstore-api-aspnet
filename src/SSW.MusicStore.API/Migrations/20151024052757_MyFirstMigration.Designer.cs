using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using SSW.MusicStore.API.Models;

namespace SSW.MusicStore.API.Migrations
{
    [DbContext(typeof(MusicStoreContext))]
    [Migration("20151024052757_MyFirstMigration")]
    partial class MyFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-beta8-15964")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasAnnotation("Relational:Name", "RoleNameHasIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.Album", b =>
                {
                    b.Property<int>("AlbumId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlbumArtUrl")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<int>("ArtistId");

                    b.Property<DateTime>("Created");

                    b.Property<int>("GenreId");

                    b.Property<decimal>("Price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 160);

                    b.HasKey("AlbumId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetUsers");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.Artist", b =>
                {
                    b.Property<int>("ArtistId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ArtistId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.CartItem", b =>
                {
                    b.Property<int>("CartItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlbumId");

                    b.Property<string>("CartId")
                        .IsRequired();

                    b.Property<int>("Count");

                    b.Property<DateTime>("DateCreated");

                    b.HasKey("CartItemId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.Genre", b =>
                {
                    b.Property<int>("GenreId");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("GenreId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 70);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 40);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 40);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 160);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 160);

                    b.Property<DateTime>("OrderDate");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 24);

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 40);

                    b.Property<decimal>("Total");

                    b.Property<string>("Username");

                    b.HasKey("OrderId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlbumId");

                    b.Property<int>("OrderId");

                    b.Property<int>("Quantity");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("OrderDetailId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SSW.MusicStore.API.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SSW.MusicStore.API.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("SSW.MusicStore.API.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.Album", b =>
                {
                    b.HasOne("SSW.MusicStore.API.Models.Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId");

                    b.HasOne("SSW.MusicStore.API.Models.Genre")
                        .WithMany()
                        .HasForeignKey("GenreId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.CartItem", b =>
                {
                    b.HasOne("SSW.MusicStore.API.Models.Album")
                        .WithMany()
                        .HasForeignKey("AlbumId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.OrderDetail", b =>
                {
                    b.HasOne("SSW.MusicStore.API.Models.Order")
                        .WithMany()
                        .HasForeignKey("OrderId");
                });
        }
    }
}
