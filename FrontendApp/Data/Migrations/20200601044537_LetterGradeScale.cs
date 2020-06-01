using Microsoft.EntityFrameworkCore.Migrations;

namespace FrontendApp.Data.Migrations
{
    public partial class LetterGradeScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LetterGradeScales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<double>(nullable: false),
                    To = table.Column<double>(nullable: false),
                    LetterGrade = table.Column<string>(nullable: true),
                    InstitutionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterGradeScales", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetterGradeScales");
        }
    }
}
