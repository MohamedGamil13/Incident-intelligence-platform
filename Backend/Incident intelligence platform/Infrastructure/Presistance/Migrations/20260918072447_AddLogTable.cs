using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistance.Migrations
{
    /// <inheritdoc />
    public partial class AddLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogLevel = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TraceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ServiceId_Timestamp",
                table: "Logs",
                columns: new[] { "ServiceId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ServiceId_Timestamp_LogLevel",
                table: "Logs",
                columns: new[] { "ServiceId", "Timestamp", "LogLevel" },
                filter: "\"LogLevel\" IN (2, 3)");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_TraceId",
                table: "Logs",
                column: "TraceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

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
    }
}
