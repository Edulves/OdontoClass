using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdontoClass.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "paciente_name",
                table: "schedules",
                newName: "pacient_name");

            migrationBuilder.RenameColumn(
                name: "paciente_info",
                table: "schedules",
                newName: "pacient_info");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pacient_name",
                table: "schedules",
                newName: "paciente_name");

            migrationBuilder.RenameColumn(
                name: "pacient_info",
                table: "schedules",
                newName: "paciente_info");
        }
    }
}
