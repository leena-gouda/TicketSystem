using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class newdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Signup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RememberMe = table.Column<bool>(type: "bit", nullable: false),
                    isCaller = table.Column<bool>(type: "bit", nullable: false),
                    isWatcher = table.Column<bool>(type: "bit", nullable: false),
                    isAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Users_LoginId",
                        column: x => x.LoginId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "watcher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_watcher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_watcher_Users_LoginId",
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
                    IsCaller = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    PendingIncidentId = table.Column<int>(type: "int", nullable: true),
                    IncidentsToReviewId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dashboards_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Impact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginId = table.Column<int>(type: "int", nullable: false),
                    DashboardModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Dashboards_DashboardModelId",
                        column: x => x.DashboardModelId,
                        principalTable: "Dashboards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Users_LoginId",
                        column: x => x.LoginId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incident",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    CallerId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    openDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    closedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedAdminId = table.Column<int>(type: "int", nullable: true),
                    DashboardModelId = table.Column<int>(type: "int", nullable: true),
                    DashboardModelId1 = table.Column<int>(type: "int", nullable: true),
                    DashboardModelId2 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incident", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incident_Admins_AssignedAdminId",
                        column: x => x.AssignedAdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Incident_Dashboards_DashboardModelId",
                        column: x => x.DashboardModelId,
                        principalTable: "Dashboards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Incident_Dashboards_DashboardModelId1",
                        column: x => x.DashboardModelId1,
                        principalTable: "Dashboards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Incident_Dashboards_DashboardModelId2",
                        column: x => x.DashboardModelId2,
                        principalTable: "Dashboards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Incident_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incident_callers_CallerId",
                        column: x => x.CallerId,
                        principalTable: "callers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PreviousComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncidentId = table.Column<int>(type: "int", nullable: false),
                    ClosedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreviousComments_Incident_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreviousComments_Users_LoginId",
                        column: x => x.LoginId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TicketWatchers",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    WatcherId = table.Column<int>(type: "int", nullable: false),
                    IncidentModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketWatchers", x => new { x.TicketId, x.WatcherId });
                    table.ForeignKey(
                        name: "FK_TicketWatchers_Incident_IncidentModelId",
                        column: x => x.IncidentModelId,
                        principalTable: "Incident",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketWatchers_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketWatchers_watcher_WatcherId",
                        column: x => x.WatcherId,
                        principalTable: "watcher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_LoginId",
                table: "Admins",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_callers_LoginId",
                table: "callers",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_CreateIncidentId",
                table: "Dashboards",
                column: "CreateIncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_IncidentsToReviewId",
                table: "Dashboards",
                column: "IncidentsToReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_PendingIncidentId",
                table: "Dashboards",
                column: "PendingIncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_userId",
                table: "Dashboards",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_AssignedAdminId",
                table: "Incident",
                column: "AssignedAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_CallerId",
                table: "Incident",
                column: "CallerId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_DashboardModelId",
                table: "Incident",
                column: "DashboardModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_DashboardModelId1",
                table: "Incident",
                column: "DashboardModelId1");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_DashboardModelId2",
                table: "Incident",
                column: "DashboardModelId2");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_TicketId",
                table: "Incident",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousComments_IncidentId",
                table: "PreviousComments",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousComments_LoginId",
                table: "PreviousComments",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DashboardModelId",
                table: "Tickets",
                column: "DashboardModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LoginId",
                table: "Tickets",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketWatchers_IncidentModelId",
                table: "TicketWatchers",
                column: "IncidentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketWatchers_WatcherId",
                table: "TicketWatchers",
                column: "WatcherId");

            migrationBuilder.CreateIndex(
                name: "IX_watcher_LoginId",
                table: "watcher",
                column: "LoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_Incident_CreateIncidentId",
                table: "Dashboards",
                column: "CreateIncidentId",
                principalTable: "Incident",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_Incident_IncidentsToReviewId",
                table: "Dashboards",
                column: "IncidentsToReviewId",
                principalTable: "Incident",
                principalColumn: "Id");

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
                name: "FK_Admins_Users_LoginId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_callers_Users_LoginId",
                table: "callers");

            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Users_userId",
                table: "Dashboards");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_LoginId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Incident_CreateIncidentId",
                table: "Dashboards");

            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Incident_IncidentsToReviewId",
                table: "Dashboards");

            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_Incident_PendingIncidentId",
                table: "Dashboards");

            migrationBuilder.DropTable(
                name: "PreviousComments");

            migrationBuilder.DropTable(
                name: "Signup");

            migrationBuilder.DropTable(
                name: "TicketWatchers");

            migrationBuilder.DropTable(
                name: "watcher");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Incident");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "callers");

            migrationBuilder.DropTable(
                name: "Dashboards");
        }
    }
}
