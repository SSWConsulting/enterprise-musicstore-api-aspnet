using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using SSW.MusicStore.Data;

namespace SSW.MusicStore.Data.Migrations
{
    [DbContext(typeof(MusicStoreContext))]
    partial class MusicStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SSW.MusicStore.Data.Entities.Album", b =>
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

            modelBuilder.Entity("SSW.MusicStore.Data.Entities.Artist", b =>
                {
                    b.Property<int>("ArtistId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ArtistId");
                });

            modelBuilder.Entity("SSW.MusicStore.Data.Entities.CartItem", b =>
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

            modelBuilder.Entity("SSW.MusicStore.Data.Entities.Genre", b =>
                {
                    b.Property<int>("GenreId");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("GenreId");
                });

            modelBuilder.Entity("SSW.MusicStore.Data.Entities.Order", b =>
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

            modelBuilder.Entity("SSW.MusicStore.Data.Entities.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlbumId");

                    b.Property<int>("OrderId");

                    b.Property<int>("Quantity");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("OrderDetailId");
                });

            modelBuilder.Entity("SSW.MusicStore.Data.Entities.Album", b =>
                {
                    b.HasOne("SSW.MusicStore.Data.Entities.Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId");

                    b.HasOne("SSW.MusicStore.Data.Entities.Genre")
                        .WithMany()
                        .HasForeignKey("GenreId");
                });

            modelBuilder.Entity("SSW.MusicStore.Data.Entities.CartItem", b =>
                {
                    b.HasOne("SSW.MusicStore.Data.Entities.Album")
                        .WithMany()
                        .HasForeignKey("AlbumId");
                });

            modelBuilder.Entity("SSW.MusicStore.Data.Entities.OrderDetail", b =>
                {
                    b.HasOne("SSW.MusicStore.Data.Entities.Order")
                        .WithMany()
                        .HasForeignKey("OrderId");
                });
        }
    }
}
