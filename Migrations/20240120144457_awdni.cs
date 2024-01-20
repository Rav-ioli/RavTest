using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDPR_dotnet_new_webapi.Migrations
{
    /// <inheritdoc />
    public partial class awdni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BeperkingId",
                table: "Onderzoeken",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Onderzoeken_BeperkingId",
                table: "Onderzoeken",
                column: "BeperkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Onderzoeken_Beperkingen_BeperkingId",
                table: "Onderzoeken",
                column: "BeperkingId",
                principalTable: "Beperkingen",
                principalColumn: "BeperkingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Onderzoeken_Beperkingen_BeperkingId",
                table: "Onderzoeken");

            migrationBuilder.DropIndex(
                name: "IX_Onderzoeken_BeperkingId",
                table: "Onderzoeken");

            migrationBuilder.DropColumn(
                name: "BeperkingId",
                table: "Onderzoeken");
        }
    }
}
