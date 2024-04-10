using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtMonitorBackend.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Programs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sports = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FunFact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsCoach = table.Column<bool>(type: "bit", nullable: false),
                    IsUser = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AdminInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ProgramID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminInfo_UserInfo_UserID",
                        column: x => x.UserID,
                        principalTable: "UserInfo",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CoachInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ProgramID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachInfo_UserInfo_UserID",
                        column: x => x.UserID,
                        principalTable: "UserInfo",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "EventInfo",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPulished = table.Column<bool>(type: "bit", nullable: false),
                    ProgramID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventInfo", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_EventInfo_UserInfo_UserID",
                        column: x => x.UserID,
                        principalTable: "UserInfo",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "GenUserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ProgramID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenUserInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenUserInfo_UserInfo_UserID",
                        column: x => x.UserID,
                        principalTable: "UserInfo",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProgramInfo",
                columns: table => new
                {
                    ProgramID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminID = table.Column<int>(type: "int", nullable: true),
                    CoachID = table.Column<int>(type: "int", nullable: true),
                    GenUserID = table.Column<int>(type: "int", nullable: true),
                    ProgramName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgramSport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramInfo", x => x.ProgramID);
                    table.ForeignKey(
                        name: "FK_ProgramInfo_AdminInfo_AdminID",
                        column: x => x.AdminID,
                        principalTable: "AdminInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProgramInfo_CoachInfo_CoachID",
                        column: x => x.CoachID,
                        principalTable: "CoachInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProgramInfo_GenUserInfo_GenUserID",
                        column: x => x.GenUserID,
                        principalTable: "GenUserInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminInfo_ProgramID",
                table: "AdminInfo",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_AdminInfo_UserID",
                table: "AdminInfo",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CoachInfo_ProgramID",
                table: "CoachInfo",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_CoachInfo_UserID",
                table: "CoachInfo",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EventInfo_ProgramID",
                table: "EventInfo",
                column: "ProgramID",
                unique: true,
                filter: "[ProgramID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EventInfo_UserID",
                table: "EventInfo",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GenUserInfo_ProgramID",
                table: "GenUserInfo",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_GenUserInfo_UserID",
                table: "GenUserInfo",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramInfo_AdminID",
                table: "ProgramInfo",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramInfo_CoachID",
                table: "ProgramInfo",
                column: "CoachID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramInfo_GenUserID",
                table: "ProgramInfo",
                column: "GenUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminInfo_ProgramInfo_ProgramID",
                table: "AdminInfo",
                column: "ProgramID",
                principalTable: "ProgramInfo",
                principalColumn: "ProgramID");

            migrationBuilder.AddForeignKey(
                name: "FK_CoachInfo_ProgramInfo_ProgramID",
                table: "CoachInfo",
                column: "ProgramID",
                principalTable: "ProgramInfo",
                principalColumn: "ProgramID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventInfo_ProgramInfo_ProgramID",
                table: "EventInfo",
                column: "ProgramID",
                principalTable: "ProgramInfo",
                principalColumn: "ProgramID");

            migrationBuilder.AddForeignKey(
                name: "FK_GenUserInfo_ProgramInfo_ProgramID",
                table: "GenUserInfo",
                column: "ProgramID",
                principalTable: "ProgramInfo",
                principalColumn: "ProgramID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminInfo_ProgramInfo_ProgramID",
                table: "AdminInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_CoachInfo_ProgramInfo_ProgramID",
                table: "CoachInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_GenUserInfo_ProgramInfo_ProgramID",
                table: "GenUserInfo");

            migrationBuilder.DropTable(
                name: "EventInfo");

            migrationBuilder.DropTable(
                name: "ProgramInfo");

            migrationBuilder.DropTable(
                name: "AdminInfo");

            migrationBuilder.DropTable(
                name: "CoachInfo");

            migrationBuilder.DropTable(
                name: "GenUserInfo");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
