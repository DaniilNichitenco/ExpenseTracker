using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTracker.API.Migrations
{
    public partial class someFixes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_People_PersonId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_PersonId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Wallets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Wallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_PersonId",
                table: "Wallets",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_People_PersonId",
                table: "Wallets",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
