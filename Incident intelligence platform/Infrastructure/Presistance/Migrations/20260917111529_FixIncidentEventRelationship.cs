using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistance.Migrations
{
    /// <inheritdoc />
    public partial class FixIncidentEventRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncidentId1",
                table: "IncidentEvents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncidentEvents_IncidentId1",
                table: "IncidentEvents",
                column: "IncidentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentEvents_Incidents_IncidentId1",
                table: "IncidentEvents",
                column: "IncidentId1",
                principalTable: "Incidents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncidentEvents_Incidents_IncidentId1",
                table: "IncidentEvents");

            migrationBuilder.DropIndex(
                name: "IX_IncidentEvents_IncidentId1",
                table: "IncidentEvents");

            migrationBuilder.DropColumn(
                name: "IncidentId1",
                table: "IncidentEvents");
        }
    }
}
