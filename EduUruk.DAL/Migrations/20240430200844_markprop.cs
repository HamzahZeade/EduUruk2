using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduUruk.DAL.Migrations
{
    public partial class markprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mark",
                table: "Test",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Test");
        }
    }
}
