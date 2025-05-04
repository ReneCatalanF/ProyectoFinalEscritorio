using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_SAP.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordenadasMesa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoordX",
                table: "Mesas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoordY",
                table: "Mesas",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordX",
                table: "Mesas");

            migrationBuilder.DropColumn(
                name: "CoordY",
                table: "Mesas");
        }
    }
}
