using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTracker.API.Migrations
{
    public partial class AddRelationBetweenUserAndItsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Topics_TopicId",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "Expenses",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_OwnerId",
                table: "UserInfos",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_OwnerId",
                table: "Topics",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Purses_OwnerId",
                table: "Purses",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_OwnerId",
                table: "Expenses",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_OwnerId",
                table: "Expenses",
                column: "OwnerId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Topics_TopicId",
                table: "Expenses",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purses_Users_OwnerId",
                table: "Purses",
                column: "OwnerId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Users_OwnerId",
                table: "Topics",
                column: "OwnerId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfos_Users_OwnerId",
                table: "UserInfos",
                column: "OwnerId",
                principalSchema: "Auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_OwnerId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Topics_TopicId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Purses_Users_OwnerId",
                table: "Purses");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Users_OwnerId",
                table: "Topics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInfos_Users_OwnerId",
                table: "UserInfos");

            migrationBuilder.DropIndex(
                name: "IX_UserInfos_OwnerId",
                table: "UserInfos");

            migrationBuilder.DropIndex(
                name: "IX_Topics_OwnerId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Purses_OwnerId",
                table: "Purses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_OwnerId",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "Expenses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Topics_TopicId",
                table: "Expenses",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
