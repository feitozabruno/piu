using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piu.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlteraTabelaUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Incubatorio",
                table: "AspNetUsers",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Incubatorio",
                table: "AspNetUsers",
                column: "Incubatorio");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Incubatorios_IncubatorioIdentificador",
                table: "AspNetUsers",
                column: "Incubatorio",
                principalTable: "Incubatorios",
                principalColumn: "Identificador",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Incubatorios_IncubatorioIdentificador",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Incubatorio",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Incubatorio",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext");
        }
    }
}
