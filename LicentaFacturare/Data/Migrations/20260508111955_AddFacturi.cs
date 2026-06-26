using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFacturare.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFacturi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facturi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartenerId = table.Column<int>(type: "int", nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numar = table.Column<int>(type: "int", nullable: false),
                    DataEmitere = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataScadenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipFactura = table.Column<int>(type: "int", nullable: false),
                    Stare = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facturi_Parteneri_PartenerId",
                        column: x => x.PartenerId,
                        principalTable: "Parteneri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiniiFactura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    ProdusId = table.Column<int>(type: "int", nullable: false),
                    Cantitate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PretUnitarFaraTVA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PretUnitarCuTVA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Valoare = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ValoareTVA = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalLinie = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiniiFactura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiniiFactura_Facturi_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiniiFactura_Produse_ProdusId",
                        column: x => x.ProdusId,
                        principalTable: "Produse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturi_PartenerId",
                table: "Facturi",
                column: "PartenerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiniiFactura_FacturaId",
                table: "LiniiFactura",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_LiniiFactura_ProdusId",
                table: "LiniiFactura",
                column: "ProdusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiniiFactura");

            migrationBuilder.DropTable(
                name: "Facturi");
        }
    }
}
