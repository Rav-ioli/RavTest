using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDPR_dotnet_new_webapi.Migrations
{
    /// <inheritdoc />
    public partial class iadafb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Datum",
                table: "Onderzoeken",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "UitvoerendBedrijfId",
                table: "Onderzoeken",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErvaringsdeskundigeId1",
                table: "GebruikerBeperkingen",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErvaringsdeskundigeId2",
                table: "GebruikerBeperkingen",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BedrijfsInformatie",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bedrijfsnaam",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ErvaringsdeskundigeOnderzoeken",
                columns: table => new
                {
                    ErvaringsdeskundigeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OnderzoekId = table.Column<int>(type: "int", nullable: false),
                    DeelnameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErvaringsdeskundigeOnderzoeken", x => new { x.ErvaringsdeskundigeId, x.OnderzoekId });
                    table.ForeignKey(
                        name: "FK_ErvaringsdeskundigeOnderzoeken_AspNetUsers_ErvaringsdeskundigeId",
                        column: x => x.ErvaringsdeskundigeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ErvaringsdeskundigeOnderzoeken_Onderzoeken_OnderzoekId",
                        column: x => x.OnderzoekId,
                        principalTable: "Onderzoeken",
                        principalColumn: "OnderzoekId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Onderzoeken_UitvoerendBedrijfId",
                table: "Onderzoeken",
                column: "UitvoerendBedrijfId");

            migrationBuilder.CreateIndex(
                name: "IX_GebruikerBeperkingen_ErvaringsdeskundigeId1",
                table: "GebruikerBeperkingen",
                column: "ErvaringsdeskundigeId1");

            migrationBuilder.CreateIndex(
                name: "IX_GebruikerBeperkingen_ErvaringsdeskundigeId2",
                table: "GebruikerBeperkingen",
                column: "ErvaringsdeskundigeId2");

            migrationBuilder.CreateIndex(
                name: "IX_ErvaringsdeskundigeOnderzoeken_OnderzoekId",
                table: "ErvaringsdeskundigeOnderzoeken",
                column: "OnderzoekId");

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerBeperkingen_AspNetUsers_ErvaringsdeskundigeId1",
                table: "GebruikerBeperkingen",
                column: "ErvaringsdeskundigeId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerBeperkingen_AspNetUsers_ErvaringsdeskundigeId2",
                table: "GebruikerBeperkingen",
                column: "ErvaringsdeskundigeId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Onderzoeken_AspNetUsers_UitvoerendBedrijfId",
                table: "Onderzoeken",
                column: "UitvoerendBedrijfId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerBeperkingen_AspNetUsers_ErvaringsdeskundigeId1",
                table: "GebruikerBeperkingen");

            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerBeperkingen_AspNetUsers_ErvaringsdeskundigeId2",
                table: "GebruikerBeperkingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Onderzoeken_AspNetUsers_UitvoerendBedrijfId",
                table: "Onderzoeken");

            migrationBuilder.DropTable(
                name: "ErvaringsdeskundigeOnderzoeken");

            migrationBuilder.DropIndex(
                name: "IX_Onderzoeken_UitvoerendBedrijfId",
                table: "Onderzoeken");

            migrationBuilder.DropIndex(
                name: "IX_GebruikerBeperkingen_ErvaringsdeskundigeId1",
                table: "GebruikerBeperkingen");

            migrationBuilder.DropIndex(
                name: "IX_GebruikerBeperkingen_ErvaringsdeskundigeId2",
                table: "GebruikerBeperkingen");

            migrationBuilder.DropColumn(
                name: "UitvoerendBedrijfId",
                table: "Onderzoeken");

            migrationBuilder.DropColumn(
                name: "ErvaringsdeskundigeId1",
                table: "GebruikerBeperkingen");

            migrationBuilder.DropColumn(
                name: "ErvaringsdeskundigeId2",
                table: "GebruikerBeperkingen");

            migrationBuilder.DropColumn(
                name: "BedrijfsInformatie",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Bedrijfsnaam",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Datum",
                table: "Onderzoeken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
