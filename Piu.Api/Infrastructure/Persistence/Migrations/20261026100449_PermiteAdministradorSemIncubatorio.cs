using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piu.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PermiteAdministradorSemIncubatorio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Incubatorio",
                table: "AspNetUsers",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Incubatorio",
                table: "AspNetUsers",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);
        }
    }
}
