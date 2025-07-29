using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class assignedIncidents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DashboardModelId2",
                table: "Incident",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incident_DashboardModelId2",
                table: "Incident",
                column: "DashboardModelId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_Dashboards_DashboardModelId2",
                table: "Incident",
                column: "DashboardModelId2",
                principalTable: "Dashboards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_Dashboards_DashboardModelId2",
                table: "Incident");

            migrationBuilder.DropIndex(
                name: "IX_Incident_DashboardModelId2",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "DashboardModelId2",
                table: "Incident");
        }
    }
}
