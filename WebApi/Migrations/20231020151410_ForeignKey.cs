using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTask_Users_UserId",
                table: "TodoTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTask",
                table: "TodoTask");

            migrationBuilder.RenameTable(
                name: "TodoTask",
                newName: "TodoTasks");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTask_UserId",
                table: "TodoTasks",
                newName: "IX_TodoTasks_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TodoTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTasks",
                table: "TodoTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Users_UserId",
                table: "TodoTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Users_UserId",
                table: "TodoTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTasks",
                table: "TodoTasks");

            migrationBuilder.RenameTable(
                name: "TodoTasks",
                newName: "TodoTask");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTasks_UserId",
                table: "TodoTask",
                newName: "IX_TodoTask_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TodoTask",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTask",
                table: "TodoTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTask_Users_UserId",
                table: "TodoTask",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
