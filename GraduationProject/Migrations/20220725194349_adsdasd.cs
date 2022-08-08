using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProject.Migrations
{
    public partial class adsdasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "soldTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConcertId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketIdent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_soldTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_soldTickets_concerts_ConcertId",
                        column: x => x.ConcertId,
                        principalTable: "concerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_soldTickets_ConcertId",
                table: "soldTickets",
                column: "ConcertId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "soldTickets");
        }
    }
}
