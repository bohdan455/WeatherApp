using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOeToManyInCities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherInformations_Cities_CityId",
                table: "WeatherInformations");

            migrationBuilder.DropIndex(
                name: "IX_WeatherInformations_CityId",
                table: "WeatherInformations");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "WeatherInformations");

            migrationBuilder.AddColumn<int>(
                name: "WeatherInformationId",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_WeatherInformationId",
                table: "Cities",
                column: "WeatherInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_WeatherInformations_WeatherInformationId",
                table: "Cities",
                column: "WeatherInformationId",
                principalTable: "WeatherInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_WeatherInformations_WeatherInformationId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_WeatherInformationId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "WeatherInformationId",
                table: "Cities");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "WeatherInformations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherInformations_CityId",
                table: "WeatherInformations",
                column: "CityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherInformations_Cities_CityId",
                table: "WeatherInformations",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
