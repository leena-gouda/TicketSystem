using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class addWatchertoPreviusComennt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WatcherId",
                table: "PreviousComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreviousComments_WatcherId",
                table: "PreviousComments",
                column: "WatcherId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousComments_watcher_WatcherId",
                table: "PreviousComments",
                column: "WatcherId",
                principalTable: "watcher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreviousComments_watcher_WatcherId",
                table: "PreviousComments");

            migrationBuilder.DropIndex(
                name: "IX_PreviousComments_WatcherId",
                table: "PreviousComments");

            migrationBuilder.DropColumn(
                name: "WatcherId",
                table: "PreviousComments");
        }
    }
}
