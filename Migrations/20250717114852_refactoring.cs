using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class refactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_watcher_WatcherId",
                table: "Incident");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousComments_watcher_WatcherId",
                table: "PreviousComments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "watcher");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "watcher");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "watcher");

            migrationBuilder.RenameColumn(
                name: "WatcherId",
                table: "PreviousComments",
                newName: "LoginId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousComments_WatcherId",
                table: "PreviousComments",
                newName: "IX_PreviousComments_LoginId");

            migrationBuilder.RenameColumn(
                name: "WatcherId",
                table: "Incident",
                newName: "callerId");

            migrationBuilder.RenameIndex(
                name: "IX_Incident_WatcherId",
                table: "Incident",
                newName: "IX_Incident_callerId");

            migrationBuilder.AddColumn<int>(
                name: "LoginId",
                table: "watcher",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isCaller",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isWatcher",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DashboardModelId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DashboardModelId",
                table: "Incident",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DashboardModelId1",
                table: "Incident",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "callers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_callers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_callers_Users_LoginId",
                        column: x => x.LoginId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dashboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    CreateIncidentId = table.Column<int>(type: "int", nullable: false),
                    IsWatcher = table.Column<bool>(type: "bit", nullable: false),
                    IsCaller = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dashboards_Incident_CreateIncidentId",
                        column: x => x.CreateIncidentId,
                        principalTable: "Incident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dashboards_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_watcher_LoginId",
                table: "watcher",
                column: "LoginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DashboardModelId",
                table: "Tickets",
                column: "DashboardModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_DashboardModelId",
                table: "Incident",
                column: "DashboardModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_DashboardModelId1",
                table: "Incident",
                column: "DashboardModelId1");

            migrationBuilder.CreateIndex(
                name: "IX_callers_LoginId",
                table: "callers",
                column: "LoginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_CreateIncidentId",
                table: "Dashboards",
                column: "CreateIncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_userId",
                table: "Dashboards",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_Dashboards_DashboardModelId",
                table: "Incident",
                column: "DashboardModelId",
                principalTable: "Dashboards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_Dashboards_DashboardModelId1",
                table: "Incident",
                column: "DashboardModelId1",
                principalTable: "Dashboards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_callers_callerId",
                table: "Incident",
                column: "callerId",
                principalTable: "callers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousComments_Users_LoginId",
                table: "PreviousComments",
                column: "LoginId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Dashboards_DashboardModelId",
                table: "Tickets",
                column: "DashboardModelId",
                principalTable: "Dashboards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_watcher_Users_LoginId",
                table: "watcher",
                column: "LoginId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_Dashboards_DashboardModelId",
                table: "Incident");

            migrationBuilder.DropForeignKey(
                name: "FK_Incident_Dashboards_DashboardModelId1",
                table: "Incident");

            migrationBuilder.DropForeignKey(
                name: "FK_Incident_callers_callerId",
                table: "Incident");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousComments_Users_LoginId",
                table: "PreviousComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Dashboards_DashboardModelId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_watcher_Users_LoginId",
                table: "watcher");

            migrationBuilder.DropTable(
                name: "callers");

            migrationBuilder.DropTable(
                name: "Dashboards");

            migrationBuilder.DropIndex(
                name: "IX_watcher_LoginId",
                table: "watcher");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_DashboardModelId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Incident_DashboardModelId",
                table: "Incident");

            migrationBuilder.DropIndex(
                name: "IX_Incident_DashboardModelId1",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "LoginId",
                table: "watcher");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "isCaller",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "isWatcher",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DashboardModelId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DashboardModelId",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "DashboardModelId1",
                table: "Incident");

            migrationBuilder.RenameColumn(
                name: "LoginId",
                table: "PreviousComments",
                newName: "WatcherId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousComments_LoginId",
                table: "PreviousComments",
                newName: "IX_PreviousComments_WatcherId");

            migrationBuilder.RenameColumn(
                name: "callerId",
                table: "Incident",
                newName: "WatcherId");

            migrationBuilder.RenameIndex(
                name: "IX_Incident_callerId",
                table: "Incident",
                newName: "IX_Incident_WatcherId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "watcher",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "watcher",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "watcher",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_watcher_WatcherId",
                table: "Incident",
                column: "WatcherId",
                principalTable: "watcher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousComments_watcher_WatcherId",
                table: "PreviousComments",
                column: "WatcherId",
                principalTable: "watcher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
