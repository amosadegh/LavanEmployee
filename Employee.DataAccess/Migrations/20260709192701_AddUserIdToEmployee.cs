using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Employee.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrations",
                columns: table => new
                {
                    AdministrationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdministrationTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrations", x => x.AdministrationId);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "Camps",
                columns: table => new
                {
                    CampId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camps", x => x.CampId);
                });

            migrationBuilder.CreateTable(
                name: "ContractTypes",
                columns: table => new
                {
                    ContractTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractTypeTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTypes", x => x.ContractTypeId);
                });

            migrationBuilder.CreateTable(
                name: "JobLocations",
                columns: table => new
                {
                    JobLocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobLocTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLocations", x => x.JobLocId);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
                columns: table => new
                {
                    JobTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTypeCode = table.Column<int>(type: "int", nullable: false),
                    JobTypeTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypes", x => x.JobTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyFields",
                columns: table => new
                {
                    StudyFieldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyFieldTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyFields", x => x.StudyFieldId);
                });

            migrationBuilder.CreateTable(
                name: "StudyLevels",
                columns: table => new
                {
                    StudyLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyLevelTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyLevels", x => x.StudyLevelId);
                });

            migrationBuilder.CreateTable(
                name: "SecondOrganizations",
                columns: table => new
                {
                    SecondOrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SecondOrganizationTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondOrganizations", x => x.SecondOrganizationId);
                    table.ForeignKey(
                        name: "FK_SecondOrganizations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EduOrientations",
                columns: table => new
                {
                    EduOrientationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyFieldId = table.Column<int>(type: "int", nullable: false),
                    EduOrientationTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EduOrientations", x => x.EduOrientationId);
                    table.ForeignKey(
                        name: "FK_EduOrientations_StudyFields_StudyFieldId",
                        column: x => x.StudyFieldId,
                        principalTable: "StudyFields",
                        principalColumn: "StudyFieldId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Family = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Father = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdentityNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NationalNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    BirthProvince = table.Column<int>(type: "int", nullable: true),
                    BirthCity = table.Column<int>(type: "int", nullable: true),
                    EntryProvince = table.Column<int>(type: "int", nullable: true),
                    EntryCity = table.Column<int>(type: "int", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HomeProvince = table.Column<int>(type: "int", nullable: true),
                    HomeCity = table.Column<int>(type: "int", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HomeTel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MilitaryService = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MilitaryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MilitaryExemptionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsuranceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsuranceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsuranceBranch = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsuranceOutOfCompany = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudyLevelId = table.Column<int>(type: "int", nullable: true),
                    StudyFieldId = table.Column<int>(type: "int", nullable: true),
                    EduOrientationId = table.Column<int>(type: "int", nullable: true),
                    ContractTypeId = table.Column<int>(type: "int", nullable: true),
                    JobId = table.Column<int>(type: "int", nullable: true),
                    AdministrationId = table.Column<int>(type: "int", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    SecondOrganizationId = table.Column<int>(type: "int", nullable: true),
                    JobLocId = table.Column<int>(type: "int", nullable: true),
                    JobTypeId = table.Column<int>(type: "int", nullable: true),
                    WorkStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PayBank = table.Column<int>(type: "int", nullable: true),
                    PayAccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PayShabaNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PayBackupBank = table.Column<int>(type: "int", nullable: true),
                    PayBackupNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PayPackupShabaNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CampId = table.Column<int>(type: "int", nullable: true),
                    RoomNo = table.Column<int>(type: "int", nullable: true),
                    RoomPhone = table.Column<int>(type: "int", nullable: true),
                    WorkPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pollution = table.Column<bool>(type: "bit", nullable: true),
                    PollutionPercent = table.Column<double>(type: "float", nullable: true),
                    PollutionRow = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Height = table.Column<bool>(type: "bit", nullable: true),
                    HeightMeter = table.Column<double>(type: "float", nullable: true),
                    Shift = table.Column<bool>(type: "bit", nullable: true),
                    Welder = table.Column<bool>(type: "bit", nullable: true),
                    HardShip = table.Column<bool>(type: "bit", nullable: true),
                    SpecialExtraordinary = table.Column<bool>(type: "bit", nullable: true),
                    ExpertiseExtraordinary = table.Column<bool>(type: "bit", nullable: true),
                    AnnualLeaveAmont = table.Column<int>(type: "int", nullable: true),
                    RemainingAnnualLeave = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_Employees_Administrations_AdministrationId",
                        column: x => x.AdministrationId,
                        principalTable: "Administrations",
                        principalColumn: "AdministrationId");
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Banks_PayBackupBank",
                        column: x => x.PayBackupBank,
                        principalTable: "Banks",
                        principalColumn: "BankId");
                    table.ForeignKey(
                        name: "FK_Employees_Banks_PayBank",
                        column: x => x.PayBank,
                        principalTable: "Banks",
                        principalColumn: "BankId");
                    table.ForeignKey(
                        name: "FK_Employees_Camps_CampId",
                        column: x => x.CampId,
                        principalTable: "Camps",
                        principalColumn: "CampId");
                    table.ForeignKey(
                        name: "FK_Employees_Cities_BirthCity",
                        column: x => x.BirthCity,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Cities_EntryCity",
                        column: x => x.EntryCity,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Cities_HomeCity",
                        column: x => x.HomeCity,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_ContractTypes_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "ContractTypes",
                        principalColumn: "ContractTypeId");
                    table.ForeignKey(
                        name: "FK_Employees_EduOrientations_EduOrientationId",
                        column: x => x.EduOrientationId,
                        principalTable: "EduOrientations",
                        principalColumn: "EduOrientationId");
                    table.ForeignKey(
                        name: "FK_Employees_JobLocations_JobLocId",
                        column: x => x.JobLocId,
                        principalTable: "JobLocations",
                        principalColumn: "JobLocId");
                    table.ForeignKey(
                        name: "FK_Employees_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobTypes",
                        principalColumn: "JobTypeId");
                    table.ForeignKey(
                        name: "FK_Employees_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId");
                    table.ForeignKey(
                        name: "FK_Employees_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId");
                    table.ForeignKey(
                        name: "FK_Employees_Organizations_SecondOrganizationId",
                        column: x => x.SecondOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId");
                    table.ForeignKey(
                        name: "FK_Employees_Provinces_BirthProvince",
                        column: x => x.BirthProvince,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Provinces_EntryProvince",
                        column: x => x.EntryProvince,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Provinces_HomeProvince",
                        column: x => x.HomeProvince,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_StudyFields_StudyFieldId",
                        column: x => x.StudyFieldId,
                        principalTable: "StudyFields",
                        principalColumn: "StudyFieldId");
                    table.ForeignKey(
                        name: "FK_Employees_StudyLevels_StudyLevelId",
                        column: x => x.StudyLevelId,
                        principalTable: "StudyLevels",
                        principalColumn: "StudyLevelId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinceId",
                table: "Cities",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_EduOrientations_StudyFieldId",
                table: "EduOrientations",
                column: "StudyFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AdministrationId",
                table: "Employees",
                column: "AdministrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BirthCity",
                table: "Employees",
                column: "BirthCity");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BirthProvince",
                table: "Employees",
                column: "BirthProvince");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CampId",
                table: "Employees",
                column: "CampId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ContractTypeId",
                table: "Employees",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EduOrientationId",
                table: "Employees",
                column: "EduOrientationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EntryCity",
                table: "Employees",
                column: "EntryCity");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EntryProvince",
                table: "Employees",
                column: "EntryProvince");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_HomeCity",
                table: "Employees",
                column: "HomeCity");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_HomeProvince",
                table: "Employees",
                column: "HomeProvince");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobId",
                table: "Employees",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobLocId",
                table: "Employees",
                column: "JobLocId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobTypeId",
                table: "Employees",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OrganizationId",
                table: "Employees",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PayBackupBank",
                table: "Employees",
                column: "PayBackupBank");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PayBank",
                table: "Employees",
                column: "PayBank");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SecondOrganizationId",
                table: "Employees",
                column: "SecondOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StudyFieldId",
                table: "Employees",
                column: "StudyFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StudyLevelId",
                table: "Employees",
                column: "StudyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondOrganizations_OrganizationId",
                table: "SecondOrganizations",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "SecondOrganizations");

            migrationBuilder.DropTable(
                name: "Administrations");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Camps");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "ContractTypes");

            migrationBuilder.DropTable(
                name: "EduOrientations");

            migrationBuilder.DropTable(
                name: "JobLocations");

            migrationBuilder.DropTable(
                name: "JobTypes");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "StudyLevels");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "StudyFields");
        }
    }
}
