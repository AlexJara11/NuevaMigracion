using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackSeccion1.Migrations
{
    /// <inheritdoc />
    public partial class SCLogica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Seccion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Curso",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Seccion");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Curso");
        }
    }
}
