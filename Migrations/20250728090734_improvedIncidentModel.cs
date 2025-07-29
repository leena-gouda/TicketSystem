using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class improvedIncidentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Incident",
                type: "datetime2",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_Admins_AssignedAdminId",
                table: "Incident");

            migrationBuilder.DropIndex(
                name: "IX_Incident_AssignedAdminId",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "AssignedAdminId",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Incident");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Incident",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Incident",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incident_AdminId",
                table: "Incident",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_Admins_AdminId",
                table: "Incident",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");
        }
    }
}
