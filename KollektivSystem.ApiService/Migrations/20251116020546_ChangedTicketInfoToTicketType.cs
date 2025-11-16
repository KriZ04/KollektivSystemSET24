using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KollektivSystem.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTicketInfoToTicketType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_TicketInfos_TicketInfoId",
                table: "PurchasedTickets");

            migrationBuilder.DropTable(
                name: "TicketInfos");

            migrationBuilder.RenameColumn(
                name: "TicketInfoId",
                table: "PurchasedTickets",
                newName: "TicketTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedTickets_TicketInfoId",
                table: "PurchasedTickets",
                newName: "IX_PurchasedTickets_TicketTypeId");

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    AliveTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedTickets_TicketTypes_TicketTypeId",
                table: "PurchasedTickets",
                column: "TicketTypeId",
                principalTable: "TicketTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_TicketTypes_TicketTypeId",
                table: "PurchasedTickets");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.RenameColumn(
                name: "TicketTypeId",
                table: "PurchasedTickets",
                newName: "TicketInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedTickets_TicketTypeId",
                table: "PurchasedTickets",
                newName: "IX_PurchasedTickets_TicketInfoId");

            migrationBuilder.CreateTable(
                name: "TicketInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AliveTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInfos", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedTickets_TicketInfos_TicketInfoId",
                table: "PurchasedTickets",
                column: "TicketInfoId",
                principalTable: "TicketInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
