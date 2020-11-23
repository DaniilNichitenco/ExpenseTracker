using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTracker.API.Migrations
{
    public partial class someFixes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purses_People_PersonId",
                table: "Purses");

            migrationBuilder.DropIndex(
                name: "IX_Purses_PersonId",
                table: "Purses");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Purses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Purses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Purses_PersonId",
                table: "Purses",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purses_People_PersonId",
                table: "Purses",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
