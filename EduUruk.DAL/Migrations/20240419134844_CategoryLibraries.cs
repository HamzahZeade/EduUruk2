using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduUruk.DAL.Migrations
{
    public partial class CategoryLibraries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryLibraryId",
                table: "Library",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CategoryLibraries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryLibraries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Library_CategoryLibraryId",
                table: "Library",
                column: "CategoryLibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Library_CategoryLibraries_CategoryLibraryId",
                table: "Library",
                column: "CategoryLibraryId",
                principalTable: "CategoryLibraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Library_CategoryLibraries_CategoryLibraryId",
                table: "Library");

            migrationBuilder.DropTable(
                name: "CategoryLibraries");

            migrationBuilder.DropIndex(
                name: "IX_Library_CategoryLibraryId",
                table: "Library");

            migrationBuilder.DropColumn(
                name: "CategoryLibraryId",
                table: "Library");
        }
    }
}
