using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KollektivSystem.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class TransitLineAndStops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_TransitLine_RouteId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_Name",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_RouteId",
                table: "Stops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransitLine",
                table: "TransitLine");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Stops");

            migrationBuilder.RenameTable(
                name: "TransitLine",
                newName: "TransitLines");

            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "Stops",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                table: "Stops",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransitLines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransitLines",
                table: "TransitLines",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TransitLineStops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TransitLineId = table.Column<int>(type: "int", nullable: false),
                    StopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitLineStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransitLineStops_Stops_StopId",
                        column: x => x.StopId,
                        principalTable: "Stops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransitLineStops_TransitLines_TransitLineId",
                        column: x => x.TransitLineId,
                        principalTable: "TransitLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransitLineStops_StopId",
                table: "TransitLineStops",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_TransitLineStops_TransitLineId_StopId",
                table: "TransitLineStops",
                columns: new[] { "TransitLineId", "StopId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransitLineStops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransitLines",
                table: "TransitLines");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Stops");

            migrationBuilder.RenameTable(
                name: "TransitLines",
                newName: "TransitLine");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Stops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Stops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransitLine",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransitLine",
                table: "TransitLine",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_Name",
                table: "Stops",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_RouteId",
                table: "Stops",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_TransitLine_RouteId",
                table: "Stops",
                column: "RouteId",
                principalTable: "TransitLine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
