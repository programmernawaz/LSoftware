using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PathLabAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabTestId = table.Column<int>(type: "int", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceRange = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestParameters_LabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestOrders_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestOrders_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestOrderId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_TestOrders_TestOrderId",
                        column: x => x.TestOrderId,
                        principalTable: "TestOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestOrderId = table.Column<int>(type: "int", nullable: false),
                    LabTestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestOrderItems_LabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestOrderItems_TestOrders_TestOrderId",
                        column: x => x.TestOrderId,
                        principalTable: "TestOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestOrderItemId = table.Column<int>(type: "int", nullable: false),
                    TestParameterId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResults_TestOrderItems_TestOrderItemId",
                        column: x => x.TestOrderItemId,
                        principalTable: "TestOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResults_TestParameters_TestParameterId",
                        column: x => x.TestParameterId,
                        principalTable: "TestParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "LabTests",
                columns: new[] { "Id", "Category", "Code", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Hematology", "CBC01", "CBC", 300m },
                    { 2, "Biochemistry", "KFT01", "KFT", 500m },
                    { 3, "Biochemistry", "LFT01", "LFT", 600m },
                    { 4, "Biochemistry", "LIP01", "Lipid Profile", 700m }
                });

            migrationBuilder.InsertData(
                table: "TestParameters",
                columns: new[] { "Id", "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[,]
                {
                    { 1, 1, "Hemoglobin", "12-16", "g/dL" },
                    { 2, 1, "WBC Count", "4000-11000", "/mm³" },
                    { 3, 1, "RBC Count", "4.5-5.9", "mill/µL" },
                    { 4, 1, "Platelets", "1.5-4.5", "lakh/µL" },
                    { 5, 1, "PCV (Hematocrit)", "36-46", "%" },
                    { 6, 2, "Urea", "15-40", "mg/dL" },
                    { 7, 2, "Creatinine", "0.6-1.3", "mg/dL" },
                    { 8, 2, "Sodium (Na)", "135-145", "mEq/L" },
                    { 9, 2, "Potassium (K)", "3.5-5.0", "mEq/L" },
                    { 10, 3, "Total Bilirubin", "0.3-1.2", "mg/dL" },
                    { 11, 3, "Direct Bilirubin", "0.0-0.3", "mg/dL" },
                    { 12, 3, "SGOT (AST)", "5-40", "U/L" },
                    { 13, 3, "SGPT (ALT)", "5-35", "U/L" },
                    { 14, 3, "Alkaline Phosphatase", "44-147", "U/L" },
                    { 15, 3, "Total Protein", "6.0-8.3", "g/dL" },
                    { 16, 3, "Albumin", "3.5-5.0", "g/dL" },
                    { 17, 4, "Total Cholesterol", "<200", "mg/dL" },
                    { 18, 4, "Triglycerides", "<150", "mg/dL" },
                    { 19, 4, "HDL Cholesterol", ">40", "mg/dL" },
                    { 20, 4, "LDL Cholesterol", "<100", "mg/dL" },
                    { 21, 4, "VLDL", "5-40", "mg/dL" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TestOrderId",
                table: "Invoices",
                column: "TestOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestOrderItems_LabTestId",
                table: "TestOrderItems",
                column: "LabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestOrderItems_TestOrderId",
                table: "TestOrderItems",
                column: "TestOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TestOrders_DoctorId",
                table: "TestOrders",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_TestOrders_PatientId",
                table: "TestOrders",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_TestParameters_LabTestId",
                table: "TestParameters",
                column: "LabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestOrderItemId",
                table: "TestResults",
                column: "TestOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestParameterId",
                table: "TestResults",
                column: "TestParameterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "TestOrderItems");

            migrationBuilder.DropTable(
                name: "TestParameters");

            migrationBuilder.DropTable(
                name: "TestOrders");

            migrationBuilder.DropTable(
                name: "LabTests");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
