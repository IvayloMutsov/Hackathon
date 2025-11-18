using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationDbContext.Migrations
{
    /// <inheritdoc />
    public partial class NameFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proffessors");

            migrationBuilder.CreateTable(
                name: "Professors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AcademicRank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScientificFiled = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    University = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false),
                    PrevParticipationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastParticipationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsecutiveCounter = table.Column<int>(type: "int", nullable: false),
                    UniIsLocal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professors", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Professors");

            migrationBuilder.CreateTable(
                name: "Proffessors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicRank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsecutiveCounter = table.Column<int>(type: "int", nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastParticipationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrevParticipationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScientificFiled = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniIsLocal = table.Column<bool>(type: "bit", nullable: false),
                    University = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proffessors", x => x.ID);
                });
        }
    }
}
