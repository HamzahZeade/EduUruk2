using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduUruk.DAL.Migrations
{
    public partial class markprop2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionMark",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionMark",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionMark",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "QuestionMark",
                table: "Answers");
        }
    }
}
