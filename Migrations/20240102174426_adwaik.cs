using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDPR_dotnet_new_webapi.Migrations
{
    /// <inheritdoc />
    public partial class adwaik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Onderzoeken",
                columns: table => new
                {
                    OnderzoekId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KorteBeschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Locatie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Beloning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoortOnderzoek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusDeelname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeBeperking = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostcodeCriteria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinLeeftijd = table.Column<int>(type: "int", nullable: true),
                    MaxLeeftijd = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Onderzoeken", x => x.OnderzoekId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Onderzoeken");
        }
    }
}
