using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KollektivSystem.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddedValueToTicketType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TicketTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TicketTypes");
        }
    }
}
