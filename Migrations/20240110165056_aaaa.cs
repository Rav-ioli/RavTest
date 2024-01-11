using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDPR_dotnet_new_webapi.Migrations
{
    /// <inheritdoc />
    public partial class aaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hulpmiddel",
                columns: table => new
                {
                    HulpmiddelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HulpmiddelNaam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hulpmiddel", x => x.HulpmiddelId);
                });

            migrationBuilder.CreateTable(
                name: "GebruikerHulpmiddel",
                columns: table => new
                {
                    GebruikerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HulpmiddelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GebruikerHulpmiddel", x => new { x.GebruikerId, x.HulpmiddelId });
                    table.ForeignKey(
                        name: "FK_GebruikerHulpmiddel_AspNetUsers_GebruikerId",
                        column: x => x.GebruikerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GebruikerHulpmiddel_Hulpmiddel_HulpmiddelId",
                        column: x => x.HulpmiddelId,
                        principalTable: "Hulpmiddel",
                        principalColumn: "HulpmiddelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GebruikerHulpmiddel_HulpmiddelId",
                table: "GebruikerHulpmiddel",
                column: "HulpmiddelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GebruikerHulpmiddel");

            migrationBuilder.DropTable(
                name: "Hulpmiddel");
        }
    }
}
