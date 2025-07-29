using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class IncidentToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncidentsToReviewId",
                table: "Dashboards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_IncidentsToReviewId",
                table: "Dashboards",
                column: "IncidentsToReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_Incident_IncidentsToReviewId",
                table: "Dashboards",
                column: "IncidentsToReviewId",
                principalTable: "Incident",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Incident_IncidentsToReviewId",
                table: "Dashboards");

            migrationBuilder.DropIndex(
                name: "IX_Dashboards_IncidentsToReviewId",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "IncidentsToReviewId",
                table: "Dashboards");
        }
    }
}
