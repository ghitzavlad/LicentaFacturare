using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFacturare.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddParteneri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parteneri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Denumire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CnpCodFiscal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NrRegComertului = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tara = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Judet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Localitate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContBancar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipPartener = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parteneri", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parteneri");
        }
    }
}
