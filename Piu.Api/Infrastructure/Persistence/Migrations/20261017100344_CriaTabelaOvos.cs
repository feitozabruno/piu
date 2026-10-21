using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piu.Api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CriaTabelaOvos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ovos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataPostura = table.Column<DateOnly>(type: "date", nullable: false),
                    NinhoIncubavel = table.Column<int>(type: "integer", nullable: false),
                    CamaIncubavel = table.Column<int>(type: "integer", nullable: false),
                    Idade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TotalRecebido = table.Column<int>(type: "integer", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TemperaturaRecebimento = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TemperaturaBau = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    PlacaCaminhao = table.Column<string>(type: "text", nullable: false),
                    Motorista = table.Column<string>(type: "text", nullable: false),
                    HoraRecebimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ovos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ovos_Lotes_LoteId",
                        column: x => x.LoteId,
                        principalTable: "Lotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ovos_LoteId",
                table: "Ovos",
                column: "LoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ovos");
        }
    }
}
