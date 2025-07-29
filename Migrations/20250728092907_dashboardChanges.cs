using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class dashboardChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Dashboards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PendingIncidentId",
                table: "Dashboards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_PendingIncidentId",
                table: "Dashboards",
                column: "PendingIncidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_Incident_PendingIncidentId",
                table: "Dashboards",
                column: "PendingIncidentId",
                principalTable: "Incident",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Incident_PendingIncidentId",
                table: "Dashboards");

            migrationBuilder.DropIndex(
                name: "IX_Dashboards_PendingIncidentId",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "PendingIncidentId",
                table: "Dashboards");
        }
    }
}
