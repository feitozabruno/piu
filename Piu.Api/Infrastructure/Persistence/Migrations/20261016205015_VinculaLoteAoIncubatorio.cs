using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piu.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class VinculaLoteAoIncubatorio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lotes_Nome",
                table: "Lotes");

            migrationBuilder.AddColumn<string>(
                name: "IncubatorioIdentificador",
                table: "Lotes",
                type: "citext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_IncubatorioIdentificador_Nome",
                table: "Lotes",
                columns: new[] { "IncubatorioIdentificador", "Nome" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lotes_Incubatorios_IncubatorioIdentificador",
                table: "Lotes",
                column: "IncubatorioIdentificador",
                principalTable: "Incubatorios",
                principalColumn: "Identificador",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lotes_Incubatorios_IncubatorioIdentificador",
                table: "Lotes");

            migrationBuilder.DropIndex(
                name: "IX_Lotes_IncubatorioIdentificador_Nome",
                table: "Lotes");

            migrationBuilder.DropColumn(
                name: "IncubatorioIdentificador",
                table: "Lotes");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_Nome",
                table: "Lotes",
                column: "Nome",
                unique: true);
        }
    }
}
