using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTracker.API.Migrations
{
    public partial class AddedOwnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Purses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "People",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Occasions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Notes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Purses");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Occasions");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Notes");
        }
    }
}
