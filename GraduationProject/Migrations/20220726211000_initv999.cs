using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProject.Migrations
{
    public partial class initv999 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "soldTickets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_soldTickets_AppUserId",
                table: "soldTickets",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_soldTickets_AspNetUsers_AppUserId",
                table: "soldTickets",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_soldTickets_AspNetUsers_AppUserId",
                table: "soldTickets");

            migrationBuilder.DropIndex(
                name: "IX_soldTickets_AppUserId",
                table: "soldTickets");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "soldTickets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
