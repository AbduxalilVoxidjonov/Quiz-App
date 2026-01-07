using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz_App.Migrations
{
    /// <inheritdoc />
    public partial class UsePrsgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BestScore",
                table: "UserProgresses");

            migrationBuilder.RenameColumn(
                name: "TotalAttempts",
                table: "UserProgresses",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "LastScore",
                table: "UserProgresses",
                newName: "AttemptNumber");

            migrationBuilder.RenameColumn(
                name: "LastAttemptDate",
                table: "UserProgresses",
                newName: "AttemptDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "UserProgresses",
                newName: "TotalAttempts");

            migrationBuilder.RenameColumn(
                name: "AttemptNumber",
                table: "UserProgresses",
                newName: "LastScore");

            migrationBuilder.RenameColumn(
                name: "AttemptDate",
                table: "UserProgresses",
                newName: "LastAttemptDate");

            migrationBuilder.AddColumn<int>(
                name: "BestScore",
                table: "UserProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
