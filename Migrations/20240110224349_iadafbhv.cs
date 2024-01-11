using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDPR_dotnet_new_webapi.Migrations
{
    /// <inheritdoc />
    public partial class iadafbhv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeelnameId",
                table: "ErvaringsdeskundigeOnderzoeken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeelnameId",
                table: "ErvaringsdeskundigeOnderzoeken",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
