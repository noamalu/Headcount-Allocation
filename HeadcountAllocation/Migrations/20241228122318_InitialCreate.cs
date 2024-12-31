using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadcountAllocation.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZone = table.Column<int>(type: "int", nullable: false),
                    JobPercentage = table.Column<double>(type: "float", nullable: false),
                    YearExp = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequiredHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageID = table.Column<int>(type: "int", nullable: false),
                    LanguageType = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    EmployeeDTOEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageID);
                    table.ForeignKey(
                        name: "FK_Languages_Employees_EmployeeDTOEmployeeId",
                        column: x => x.EmployeeDTOEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    TimeZone = table.Column<int>(type: "int", nullable: false),
                    ForeignLanguages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearsExperience = table.Column<int>(type: "int", nullable: false),
                    JobPercentage = table.Column<double>(type: "float", nullable: false),
                    EmployeeDTOEmployeeId = table.Column<int>(type: "int", nullable: true),
                    ProjectDTOProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => new { x.ProjectId, x.EmployeeId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Roles_Employees_EmployeeDTOEmployeeId",
                        column: x => x.EmployeeDTOEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_Roles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_Roles_Projects_ProjectDTOProjectId",
                        column: x => x.ProjectDTOProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    SkillType = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    EmployeeDTOEmployeeId = table.Column<int>(type: "int", nullable: true),
                    RoleDTOEmployeeId = table.Column<int>(type: "int", nullable: true),
                    RoleDTOProjectId = table.Column<int>(type: "int", nullable: true),
                    RoleDTORoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK_Skills_Employees_EmployeeDTOEmployeeId",
                        column: x => x.EmployeeDTOEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Skills_Roles_RoleDTOProjectId_RoleDTOEmployeeId_RoleDTORoleId",
                        columns: x => new { x.RoleDTOProjectId, x.RoleDTOEmployeeId, x.RoleDTORoleId },
                        principalTable: "Roles",
                        principalColumns: new[] { "ProjectId", "EmployeeId", "RoleId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Languages_EmployeeDTOEmployeeId",
                table: "Languages",
                column: "EmployeeDTOEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_EmployeeDTOEmployeeId",
                table: "Roles",
                column: "EmployeeDTOEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_EmployeeId",
                table: "Roles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ProjectDTOProjectId",
                table: "Roles",
                column: "ProjectDTOProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_EmployeeDTOEmployeeId",
                table: "Skills",
                column: "EmployeeDTOEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_RoleDTOProjectId_RoleDTOEmployeeId_RoleDTORoleId",
                table: "Skills",
                columns: new[] { "RoleDTOProjectId", "RoleDTOEmployeeId", "RoleDTORoleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
