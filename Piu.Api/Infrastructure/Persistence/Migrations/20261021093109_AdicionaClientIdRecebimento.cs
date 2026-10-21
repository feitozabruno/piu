using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piu.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaClientIdRecebimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Ovos",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql("UPDATE \"Ovos\" SET \"ClientId\" = \"Id\" WHERE \"ClientId\" IS NULL;");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "Ovos",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ovos_ClientId",
                table: "Ovos",
                column: "ClientId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ovos_ClientId",
                table: "Ovos");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Ovos");
        }
    }
}
