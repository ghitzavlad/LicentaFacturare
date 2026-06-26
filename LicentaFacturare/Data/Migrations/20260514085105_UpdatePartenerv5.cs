using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFacturare.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePartenerv5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Parteneri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Parteneri",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Parteneri");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Parteneri");
        }
    }
}
