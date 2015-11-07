using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

using SSW.MusicStore.API.Models;

namespace SSW.MusicStore.API.Migrations
{
    public partial class Added_Cart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CartId",
                table: "CartItem",
                type: "nvarchar(500)",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<string>(nullable: false, type: "nvarchar(500)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);
                });
            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Cart_CartId",
                table: "CartItem",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "CartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_CartItem_Cart_CartId", table: "CartItem");
            migrationBuilder.DropTable("Cart");
        }
    }
}
