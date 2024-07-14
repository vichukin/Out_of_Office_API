using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Out_of_Office_API.Migrations
{
    /// <inheritdoc />
    public partial class someChanges4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalRequests_AspNetUsers_ApproverId1",
                table: "ApprovalRequests");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalRequests_ApproverId1",
                table: "ApprovalRequests");

            migrationBuilder.DropColumn(
                name: "ApproverId1",
                table: "ApprovalRequests");

            migrationBuilder.AlterColumn<string>(
                name: "ApproverId",
                table: "ApprovalRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_ApproverId",
                table: "ApprovalRequests",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalRequests_AspNetUsers_ApproverId",
                table: "ApprovalRequests",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalRequests_AspNetUsers_ApproverId",
                table: "ApprovalRequests");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalRequests_ApproverId",
                table: "ApprovalRequests");

            migrationBuilder.AlterColumn<int>(
                name: "ApproverId",
                table: "ApprovalRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproverId1",
                table: "ApprovalRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_ApproverId1",
                table: "ApprovalRequests",
                column: "ApproverId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalRequests_AspNetUsers_ApproverId1",
                table: "ApprovalRequests",
                column: "ApproverId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
