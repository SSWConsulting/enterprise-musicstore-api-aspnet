using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using SSW.MusicStore.API.Models;

namespace SSW.MusicStore.API.Migrations
{
    [DbContext(typeof(MusicStoreContext))]
    [Migration("20151107064116_Added_Cart")]
    partial class Added_Cart
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SSW.MusicStore.API.Models.Album", b =>
                {
                    b.Property<int>("AlbumId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlbumArtUrl")
                        .Annotation("MaxLength", 1024);

                    b.Property<int>("ArtistId");

                    b.Property<DateTime>("Created");

                    b.Property<int>("GenreId");

                    b.Property<decimal>("Price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .Annotation("MaxLength", 160);

                    b.HasKey("AlbumId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.Artist", b =>
                {
                    b.Property<int>("ArtistId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ArtistId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.Cart", b =>
                {
                    b.Property<string>("CartId")
                        .Annotation("MaxLength", 500);

                    b.HasKey("CartId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.CartItem", b =>
                {
                    b.Property<int>("CartItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlbumId");

                    b.Property<string>("CartId")
                        .IsRequired()
                        .Annotation("MaxLength", 500);

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
                        .Annotation("MaxLength", 70);

                    b.Property<string>("City")
                        .IsRequired()
                        .Annotation("MaxLength", 40);

                    b.Property<string>("Country")
                        .IsRequired()
                        .Annotation("MaxLength", 40);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .Annotation("MaxLength", 160);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .Annotation("MaxLength", 160);

                    b.Property<DateTime>("OrderDate");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .Annotation("MaxLength", 24);

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .Annotation("MaxLength", 10);

                    b.Property<string>("State")
                        .IsRequired()
                        .Annotation("MaxLength", 40);

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

            modelBuilder.Entity("SSW.MusicStore.API.Models.Album", b =>
                {
                    b.HasOne("SSW.MusicStore.API.Models.Artist")
                        .WithMany()
                        .ForeignKey("ArtistId");

                    b.HasOne("SSW.MusicStore.API.Models.Genre")
                        .WithMany()
                        .ForeignKey("GenreId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.CartItem", b =>
                {
                    b.HasOne("SSW.MusicStore.API.Models.Album")
                        .WithMany()
                        .ForeignKey("AlbumId");

                    b.HasOne("SSW.MusicStore.API.Models.Cart")
                        .WithMany()
                        .ForeignKey("CartId");
                });

            modelBuilder.Entity("SSW.MusicStore.API.Models.OrderDetail", b =>
                {
                    b.HasOne("SSW.MusicStore.API.Models.Order")
                        .WithMany()
                        .ForeignKey("OrderId");
                });
        }
    }
}
