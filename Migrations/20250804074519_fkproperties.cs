using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class fkproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_callers_callerId",
                table: "Incident");

            migrationBuilder.RenameColumn(
                name: "callerId",
                table: "Incident",
                newName: "CallerId");

            migrationBuilder.RenameIndex(
                name: "IX_Incident_callerId",
                table: "Incident",
                newName: "IX_Incident_CallerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_callers_CallerId",
                table: "Incident",
                column: "CallerId",
                principalTable: "callers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_callers_CallerId",
                table: "Incident");

            migrationBuilder.RenameColumn(
                name: "CallerId",
                table: "Incident",
                newName: "callerId");

            migrationBuilder.RenameIndex(
                name: "IX_Incident_CallerId",
                table: "Incident",
                newName: "IX_Incident_callerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_callers_callerId",
                table: "Incident",
                column: "callerId",
                principalTable: "callers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
