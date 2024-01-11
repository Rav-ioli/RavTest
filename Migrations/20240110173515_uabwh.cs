using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDPR_dotnet_new_webapi.Migrations
{
    /// <inheritdoc />
    public partial class uabwh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerBeperkingen_AspNetUsers_GebruikerId",
                table: "GebruikerBeperkingen");

            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerHulpmiddelen_AspNetUsers_GebruikerId",
                table: "GebruikerHulpmiddelen");

            migrationBuilder.RenameColumn(
                name: "GebruikerId",
                table: "GebruikerHulpmiddelen",
                newName: "ErvaringsdeskundigeId");

            migrationBuilder.RenameColumn(
                name: "GebruikerId",
                table: "GebruikerBeperkingen",
                newName: "ErvaringsdeskundigeId");

            migrationBuilder.AddColumn<string>(
                name: "Achternaam",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeschikbareTijden",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmailVoogd",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GeboorteDatum",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MagCommercieelBenaderdWorden",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NaamVoogd",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefoonnummerVoogd",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoorkeurBenadering",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Voornaam",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerBeperkingen_AspNetUsers_ErvaringsdeskundigeId",
                table: "GebruikerBeperkingen",
                column: "ErvaringsdeskundigeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerHulpmiddelen_AspNetUsers_ErvaringsdeskundigeId",
                table: "GebruikerHulpmiddelen",
                column: "ErvaringsdeskundigeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerBeperkingen_AspNetUsers_ErvaringsdeskundigeId",
                table: "GebruikerBeperkingen");

            migrationBuilder.DropForeignKey(
                name: "FK_GebruikerHulpmiddelen_AspNetUsers_ErvaringsdeskundigeId",
                table: "GebruikerHulpmiddelen");

            migrationBuilder.DropColumn(
                name: "Achternaam",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BeschikbareTijden",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailVoogd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GeboorteDatum",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MagCommercieelBenaderdWorden",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NaamVoogd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TelefoonnummerVoogd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VoorkeurBenadering",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Voornaam",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ErvaringsdeskundigeId",
                table: "GebruikerHulpmiddelen",
                newName: "GebruikerId");

            migrationBuilder.RenameColumn(
                name: "ErvaringsdeskundigeId",
                table: "GebruikerBeperkingen",
                newName: "GebruikerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerBeperkingen_AspNetUsers_GebruikerId",
                table: "GebruikerBeperkingen",
                column: "GebruikerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GebruikerHulpmiddelen_AspNetUsers_GebruikerId",
                table: "GebruikerHulpmiddelen",
                column: "GebruikerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
