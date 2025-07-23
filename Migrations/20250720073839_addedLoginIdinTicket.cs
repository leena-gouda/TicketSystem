using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class addedLoginIdinTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LoginId",
                table: "Tickets",
                column: "LoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_LoginId",
                table: "Tickets",
                column: "LoginId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_LoginId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_LoginId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LoginId",
                table: "Tickets");
        }
    }
}
