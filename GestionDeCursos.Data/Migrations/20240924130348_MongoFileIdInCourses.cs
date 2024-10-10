using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDeCursos.Data.Migrations
{
    /// <inheritdoc />
    public partial class MongoFileIdInCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MongoFileId",
                schema: "Management",
                table: "Courses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MongoFileId",
                schema: "Management",
                table: "Courses");
        }
    }
}
