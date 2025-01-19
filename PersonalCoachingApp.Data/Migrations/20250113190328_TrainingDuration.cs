using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalCoachingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class TrainingDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingDuration",
                table: "Packages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainingDuration",
                table: "Packages");
        }
    }
}
