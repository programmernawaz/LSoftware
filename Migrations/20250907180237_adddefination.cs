using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PathLabAPI.Migrations
{
    /// <inheritdoc />
    public partial class adddefination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Definition",
                table: "TestParameters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "MaxValue",
                table: "TestParameters",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinValue",
                table: "TestParameters",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Definition", "MaxValue", "MinValue" },
                values: new object[] { "Hemoglobin measures oxygen-carrying protein in red blood cells; low levels may indicate anemia.", null, null });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Definition", "MaxValue", "MinValue" },
                values: new object[] { "White blood cell count indicates immune system activity; high levels suggest infection or inflammation.", null, null });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Definition", "MaxValue", "MinValue" },
                values: new object[] { "Red blood cell count measures number of red cells; abnormalities may indicate anemia or polycythemia.", null, null });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Definition", "MaxValue", "MinValue" },
                values: new object[] { "Platelets help in blood clotting; low levels increase bleeding risk, high levels increase clot risk.", null, null });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Definition", "MaxValue", "MinValue" },
                values: new object[] { "Hematocrit measures proportion of blood occupied by red cells; low values suggest anemia.", null, null });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "MCV indicates average size of red blood cells; useful for anemia classification.", 1, null, null, "MCV (Mean Corpuscular Volume)", "80-100", "fL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "MCH shows average hemoglobin per red cell; helps identify anemia type.", 1, null, null, "MCH (Mean Corpuscular Hemoglobin)", "27-32", "pg" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "MCHC indicates hemoglobin concentration within red cells; abnormal in anemia.", 1, null, null, "MCHC (Mean Corpuscular Hemoglobin Concentration)", "32-36", "g/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "RDW measures variation in red cell size; high values suggest mixed anemia.", 1, null, null, "RDW (Red Cell Distribution Width)", "11-15", "%" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Neutrophils are first-line defense against infection; high in bacterial infections.", 1, null, null, "Neutrophils", "40-60", "%" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Lymphocytes are key for immunity; high in viral infections.", 1, null, null, "Lymphocytes", "20-40", "%" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Monocytes fight chronic infection and inflammation.", 1, null, null, "Monocytes", "2-8", "%" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Eosinophils increase in allergies and parasitic infections.", 1, null, null, "Eosinophils", "1-6", "%" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Basophils release histamine in allergic reactions.", 1, null, null, "Basophils", "0-1", "%" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Absolute neutrophil count evaluates infection-fighting capacity.", 1, null, null, "Absolute Neutrophil Count", "1500-8000", "/mm³" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Absolute lymphocyte count reflects immune strength.", 1, null, null, "Absolute Lymphocyte Count", "1000-4800", "/mm³" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Absolute monocyte count helps assess chronic infection or inflammation.", 1, null, null, "Absolute Monocyte Count", "200-800", "/mm³" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Absolute eosinophil count increases in allergies and asthma.", 1, null, null, "Absolute Eosinophil Count", "50-500", "/mm³" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { "Absolute basophil count is rarely elevated; linked to allergic responses.", 1, null, null, "Absolute Basophil Count", "0-200", "/mm³" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange" },
                values: new object[] { "Blood Urea indicates kidney’s ability to remove waste products.", 2, null, null, "Blood Urea", "15-40" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange" },
                values: new object[] { "Creatinine level helps assess kidney filtration efficiency.", 2, null, null, "Serum Creatinine", "0.6-1.2" });

            migrationBuilder.InsertData(
                table: "TestParameters",
                columns: new[] { "Id", "Definition", "LabTestId", "MaxValue", "MinValue", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[,]
                {
                    { 22, "Uric acid measures purine metabolism; high levels may cause gout or kidney issues.", 2, null, null, "Uric Acid", "Male: 3.4-7.0, Female: 2.4-6.0", "mg/dL" },
                    { 23, "BUN reflects nitrogen waste in blood; used for kidney function testing.", 2, null, null, "Blood Urea Nitrogen (BUN)", "7-20", "mg/dL" },
                    { 24, "Ratio helps differentiate between kidney and non-kidney causes of dysfunction.", 2, null, null, "BUN/Creatinine Ratio", "10:1 – 20:1", "Ratio" },
                    { 25, "Sodium is essential for fluid balance and nerve function.", 2, null, null, "Sodium (Na⁺)", "135-145", "mmol/L" },
                    { 26, "Potassium is important for heart and muscle function.", 2, null, null, "Potassium (K⁺)", "3.5-5.0", "mmol/L" },
                    { 27, "Chloride helps maintain acid-base balance and hydration.", 2, null, null, "Chloride (Cl⁻)", "96-106", "mmol/L" },
                    { 28, "Calcium is needed for bones, nerves, and muscles.", 2, null, null, "Calcium (Ca²⁺)", "8.5-10.5", "mg/dL" },
                    { 29, "Phosphorus is essential for energy production and bone health.", 2, null, null, "Phosphorus", "2.5-4.5", "mg/dL" },
                    { 30, "eGFR estimates kidney filtration rate; key marker for CKD.", 2, null, null, "eGFR", ">90", "mL/min/1.73m²" },
                    { 31, "Total bilirubin measures breakdown of red blood cells; high levels indicate jaundice.", 3, null, null, "Total Bilirubin", "0.3-1.2", "mg/dL" },
                    { 32, "Direct bilirubin indicates liver’s ability to excrete bile.", 3, null, null, "Direct Bilirubin", "0.0-0.3", "mg/dL" },
                    { 33, "Indirect bilirubin is unconjugated; imbalance may indicate hemolysis.", 3, null, null, "Indirect Bilirubin", "0.2-0.9", "mg/dL" },
                    { 34, "AST enzyme helps evaluate liver and heart health.", 3, null, null, "SGOT (AST)", "Male: <40, Female: <35", "U/L" },
                    { 35, "ALT enzyme is specific for liver injury.", 3, null, null, "SGPT (ALT)", "Male: <40, Female: <35", "U/L" },
                    { 36, "ALP helps detect liver, bile duct, and bone disorders.", 3, null, null, "Alkaline Phosphatase (ALP)", "44-147", "U/L" },
                    { 37, "GGT is elevated in alcohol or bile duct-related liver diseases.", 3, null, null, "Gamma GT (GGT)", "Male: 15-85, Female: 5-55", "U/L" },
                    { 38, "Total protein indicates nutritional status and liver function.", 3, null, null, "Total Protein", "6.0-8.3", "g/dL" },
                    { 39, "Albumin maintains osmotic pressure and transports substances.", 3, null, null, "Albumin", "3.5-5.0", "g/dL" },
                    { 40, "Globulins play roles in immunity and transport.", 3, null, null, "Globulin", "2.0-3.5", "g/dL" },
                    { 41, "Albumin/Globulin ratio helps in diagnosing liver and kidney disease.", 3, null, null, "A/G Ratio", "1.2-2.2", "Ratio" },
                    { 42, "Total cholesterol measures overall fat in the blood; high levels increase heart risk.", 4, null, null, "Total Cholesterol", "<200", "mg/dL" },
                    { 43, "Triglycerides are stored fats; high levels increase risk of heart disease.", 4, null, null, "Triglycerides", "<150", "mg/dL" },
                    { 44, "HDL is ‘good’ cholesterol that protects against heart disease.", 4, null, null, "HDL Cholesterol", "Male: >40, Female: >50", "mg/dL" },
                    { 45, "LDL is ‘bad’ cholesterol; high levels lead to artery blockage.", 4, null, null, "LDL Cholesterol", "<100 Optimal", "mg/dL" },
                    { 46, "VLDL carries triglycerides; high levels indicate high fat in blood.", 4, null, null, "VLDL Cholesterol", "5-30", "mg/dL" },
                    { 47, "Non-HDL cholesterol is a better predictor of heart risk than LDL alone.", 4, null, null, "Non-HDL Cholesterol", "<130", "mg/dL" },
                    { 48, "Cholesterol/HDL ratio helps evaluate heart risk.", 4, null, null, "TC/HDL Ratio", "<5", "Ratio" },
                    { 49, "LDL/HDL ratio is used to assess risk of cardiovascular disease.", 4, null, null, "LDL/HDL Ratio", "<3.5", "Ratio" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DropColumn(
                name: "Definition",
                table: "TestParameters");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "TestParameters");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "TestParameters");

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 2, "Urea", "15-40", "mg/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 2, "Creatinine", "0.6-1.3", "mg/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 2, "Sodium (Na)", "135-145", "mEq/L" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 2, "Potassium (K)", "3.5-5.0", "mEq/L" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 3, "Total Bilirubin", "0.3-1.2", "mg/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 3, "Direct Bilirubin", "0.0-0.3", "mg/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 3, "SGOT (AST)", "5-40", "U/L" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 3, "SGPT (ALT)", "5-35", "U/L" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 3, "Alkaline Phosphatase", "44-147", "U/L" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 3, "Total Protein", "6.0-8.3", "g/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 3, "Albumin", "3.5-5.0", "g/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 4, "Total Cholesterol", "<200", "mg/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 4, "Triglycerides", "<150", "mg/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange", "Unit" },
                values: new object[] { 4, "HDL Cholesterol", ">40", "mg/dL" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange" },
                values: new object[] { 4, "LDL Cholesterol", "<100" });

            migrationBuilder.UpdateData(
                table: "TestParameters",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "LabTestId", "ParameterName", "ReferenceRange" },
                values: new object[] { 4, "VLDL", "5-40" });
        }
    }
}
