using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_watcher_LoginId",
                table: "watcher");

            migrationBuilder.DropIndex(
                name: "IX_callers_LoginId",
                table: "callers");

            migrationBuilder.CreateIndex(
                name: "IX_watcher_LoginId",
                table: "watcher",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_callers_LoginId",
                table: "callers",
                column: "LoginId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_watcher_LoginId",
                table: "watcher");

            migrationBuilder.DropIndex(
                name: "IX_callers_LoginId",
                table: "callers");

            migrationBuilder.CreateIndex(
                name: "IX_watcher_LoginId",
                table: "watcher",
                column: "LoginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_callers_LoginId",
                table: "callers",
                column: "LoginId",
                unique: true);
        }
    }
}
