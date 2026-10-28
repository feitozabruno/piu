using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piu.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaAtivoAosLotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Lotes",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Lotes");
        }
    }
}
