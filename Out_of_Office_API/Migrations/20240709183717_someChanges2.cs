using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Out_of_Office_API.Migrations
{
    /// <inheritdoc />
    public partial class someChanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_ProjectManagerId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectManagerId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId1",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectManagerId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectManagerId",
                table: "Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagerId1",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId1",
                table: "Projects",
                column: "ProjectManagerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_ProjectManagerId1",
                table: "Projects",
                column: "ProjectManagerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
