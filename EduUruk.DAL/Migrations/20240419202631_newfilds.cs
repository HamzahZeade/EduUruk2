using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduUruk.DAL.Migrations
{
    public partial class newfilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Library",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceType",
                table: "Library",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusDocument",
                table: "Library",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Library");

            migrationBuilder.DropColumn(
                name: "PriceType",
                table: "Library");

            migrationBuilder.DropColumn(
                name: "StatusDocument",
                table: "Library");
        }
    }
}
