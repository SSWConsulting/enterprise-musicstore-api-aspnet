using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace SSW.MusicStore.Data.Migrations
{
    public partial class AddedTransactionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "TransactionId", table: "Order");
        }
    }
}
