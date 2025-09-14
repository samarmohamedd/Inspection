using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspection.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BusinessType = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inspectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmployeeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CertificationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CertificationExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspectors", x => x.Id);
                });
        }

            migrationBuilder.CreateTable(
                name: "InspectionVisits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    InspectorId = table.Column<int>(type: "int", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectionType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OverallScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FollowUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionVisits_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionVisits_Inspectors_InspectorId",
                        column: x => x.InspectorId,
                        principalTable: "Inspectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspectionResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionVisitId = table.Column<int>(type: "int", nullable: false),
                    InspectionItemId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CorrectiveAction = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CorrectiveActionDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasPhotographicEvidence = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionResults_InspectionItems_InspectionItemId",
                        column: x => x.InspectionItemId,
                        principalTable: "InspectionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionResults_InspectionVisits_InspectionVisitId",
                        column: x => x.InspectionVisitId,
                        principalTable: "InspectionVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_Businesses_BusinessType",
                table: "Businesses",
                column: "BusinessType");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_City",
                table: "Businesses",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_Name",
                table: "Businesses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_RegistrationNumber",
                table: "Businesses",
                column: "RegistrationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_Category",
                table: "InspectionItems",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_DisplayOrder",
                table: "InspectionItems",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_IsActive",
                table: "InspectionItems",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionResults_InspectionItemId",
                table: "InspectionResults",
                column: "InspectionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionResults_InspectionVisitId",
                table: "InspectionResults",
                column: "InspectionVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionResults_InspectionVisitId_InspectionItemId",
                table: "InspectionResults",
                columns: new[] { "InspectionVisitId", "InspectionItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionResults_Status",
                table: "InspectionResults",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionVisits_BusinessId",
                table: "InspectionVisits",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionVisits_InspectionDate",
                table: "InspectionVisits",
                column: "InspectionDate");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionVisits_InspectionType",
                table: "InspectionVisits",
                column: "InspectionType");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionVisits_InspectorId",
                table: "InspectionVisits",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionVisits_Status",
                table: "InspectionVisits",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Inspectors_Email",
                table: "Inspectors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inspectors_EmployeeId",
                table: "Inspectors",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspectors_IsActive",
                table: "Inspectors",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "InspectionResults");
            migrationBuilder.DropTable(name: "InspectionVisits");
            migrationBuilder.DropTable(name: "InspectionItems");
            migrationBuilder.DropTable(name: "Businesses");
            migrationBuilder.DropTable(name: "Inspectors");
        }
    }
}
