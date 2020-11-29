using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTracker.API.Migrations
{
    public partial class CreatedTopics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_People_PersonId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Occasions_People_PersonId",
                table: "Occasions");

            migrationBuilder.DropIndex(
                name: "IX_Occasions_PersonId",
                table: "Occasions");

            migrationBuilder.DropIndex(
                name: "IX_Notes_PersonId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Occasions");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Expense",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expense_TopicId",
                table: "Expense",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Topics_TopicId",
                table: "Expense",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Topics_TopicId",
                table: "Expense");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Expense_TopicId",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Expense");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Occasions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Occasions_PersonId",
                table: "Occasions",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PersonId",
                table: "Notes",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_People_PersonId",
                table: "Notes",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Occasions_People_PersonId",
                table: "Occasions",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
