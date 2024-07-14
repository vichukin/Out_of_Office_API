using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Out_of_Office_API.Migrations
{
    /// <inheritdoc />
    public partial class someChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_EmployeeId1",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_EmployeeId1",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "LeaveRequests");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "LeaveRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeId",
                table: "LeaveRequests",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_EmployeeId",
                table: "LeaveRequests",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_EmployeeId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_EmployeeId",
                table: "LeaveRequests");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "LeaveRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId1",
                table: "LeaveRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeId1",
                table: "LeaveRequests",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_EmployeeId1",
                table: "LeaveRequests",
                column: "EmployeeId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
