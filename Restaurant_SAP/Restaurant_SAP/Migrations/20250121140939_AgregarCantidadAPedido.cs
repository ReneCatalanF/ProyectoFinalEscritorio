using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_SAP.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCantidadAPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Pedidos");
        }
    }
}
