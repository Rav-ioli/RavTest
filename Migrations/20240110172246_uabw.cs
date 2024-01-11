using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDPR_dotnet_new_webapi.Migrations
{
    /// <inheritdoc />
    public partial class uabw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerHulpmiddel_AspNetUsers_GebruikerId",
                table: "GebruikerHulpmiddel");

            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerHulpmiddel_Hulpmiddel_HulpmiddelId",
                table: "GebruikerHulpmiddel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hulpmiddel",
                table: "Hulpmiddel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GebruikerHulpmiddel",
                table: "GebruikerHulpmiddel");

            migrationBuilder.RenameTable(
                name: "Hulpmiddel",
                newName: "Hulpmiddelen");

            migrationBuilder.RenameTable(
                name: "GebruikerHulpmiddel",
                newName: "GebruikerHulpmiddelen");

            migrationBuilder.RenameIndex(
                name: "IX_GebruikerHulpmiddel_HulpmiddelId",
                table: "GebruikerHulpmiddelen",
                newName: "IX_GebruikerHulpmiddelen_HulpmiddelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hulpmiddelen",
                table: "Hulpmiddelen",
                column: "HulpmiddelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GebruikerHulpmiddelen",
                table: "GebruikerHulpmiddelen",
                columns: new[] { "GebruikerId", "HulpmiddelId" });

            migrationBuilder.CreateTable(
                name: "Beperkingen",
                columns: table => new
                {
                    BeperkingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeperkingNaam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beperkingen", x => x.BeperkingId);
                });

            migrationBuilder.CreateTable(
                name: "GebruikerBeperkingen",
                columns: table => new
                {
                    GebruikerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BeperkingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GebruikerBeperkingen", x => new { x.GebruikerId, x.BeperkingId });
                    table.ForeignKey(
                        name: "FK_GebruikerBeperkingen_AspNetUsers_GebruikerId",
                        column: x => x.GebruikerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GebruikerBeperkingen_Beperkingen_BeperkingId",
                        column: x => x.BeperkingId,
                        principalTable: "Beperkingen",
                        principalColumn: "BeperkingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GebruikerBeperkingen_BeperkingId",
                table: "GebruikerBeperkingen",
                column: "BeperkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerHulpmiddelen_AspNetUsers_GebruikerId",
                table: "GebruikerHulpmiddelen",
                column: "GebruikerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerHulpmiddelen_Hulpmiddelen_HulpmiddelId",
                table: "GebruikerHulpmiddelen",
                column: "HulpmiddelId",
                principalTable: "Hulpmiddelen",
                principalColumn: "HulpmiddelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerHulpmiddelen_AspNetUsers_GebruikerId",
                table: "GebruikerHulpmiddelen");

            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerHulpmiddelen_Hulpmiddelen_HulpmiddelId",
                table: "GebruikerHulpmiddelen");

            migrationBuilder.DropTable(
                name: "GebruikerBeperkingen");

            migrationBuilder.DropTable(
                name: "Beperkingen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hulpmiddelen",
                table: "Hulpmiddelen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GebruikerHulpmiddelen",
                table: "GebruikerHulpmiddelen");

            migrationBuilder.RenameTable(
                name: "Hulpmiddelen",
                newName: "Hulpmiddel");

            migrationBuilder.RenameTable(
                name: "GebruikerHulpmiddelen",
                newName: "GebruikerHulpmiddel");

            migrationBuilder.RenameIndex(
                name: "IX_GebruikerHulpmiddelen_HulpmiddelId",
                table: "GebruikerHulpmiddel",
                newName: "IX_GebruikerHulpmiddel_HulpmiddelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hulpmiddel",
                table: "Hulpmiddel",
                column: "HulpmiddelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GebruikerHulpmiddel",
                table: "GebruikerHulpmiddel",
                columns: new[] { "GebruikerId", "HulpmiddelId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerHulpmiddel_AspNetUsers_GebruikerId",
                table: "GebruikerHulpmiddel",
                column: "GebruikerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerHulpmiddel_Hulpmiddel_HulpmiddelId",
                table: "GebruikerHulpmiddel",
                column: "HulpmiddelId",
                principalTable: "Hulpmiddel",
                principalColumn: "HulpmiddelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
