using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piu.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTabelaIncubatorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "Incubatorios",
                columns: table => new
                {
                    Identificador = table.Column<string>(type: "citext", nullable: false),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Sigla = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incubatorios", x => x.Identificador);
                    table.CheckConstraint("CK_Incubatorios_Identificador_Tamanho", "char_length(\"Identificador\") BETWEEN 3 AND 50");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incubatorios");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");
        }
    }
}
