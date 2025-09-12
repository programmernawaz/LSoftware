using Microsoft.EntityFrameworkCore;
using PathLabAPI.Entities;

namespace PathLabAPI.Data
{
    public class PathLabDbContext : DbContext
    {
        public PathLabDbContext(DbContextOptions<PathLabDbContext> options) : base(options) { }

        public DbSet<AppUser> AppUsers  =>Set<AppUser>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<LabTest> LabTests => Set<LabTest>();
        public DbSet<TestParameter> TestParameters => Set<TestParameter>();
        public DbSet<TestOrder> TestOrders => Set<TestOrder>();
        public DbSet<TestOrderItem> TestOrderItems => Set<TestOrderItem>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<Invoice> Invoices => Set<Invoice>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== Relationships =====

            // TestParameter -> LabTest (many-to-one)
            // TestParameter -> LabTest  (avoid cascades from LabTest)
            modelBuilder.Entity<TestParameter>()
                .HasOne(p => p.LabTest)
                .WithMany(t => t.Parameters)
                .HasForeignKey(p => p.LabTestId)
                .OnDelete(DeleteBehavior.Restrict); // was Cascade

            // TestOrderItem -> TestOrder (keep cascade so orders clean up items)
            modelBuilder.Entity<TestOrderItem>()
                .HasOne(i => i.TestOrder)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.TestOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // TestResult -> TestOrderItem (keep cascade so items clean up results)
            modelBuilder.Entity<TestResult>()
                .HasOne(r => r.TestOrderItem)
                .WithMany(i => i.Results)
                .HasForeignKey(r => r.TestOrderItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // TestResult -> TestParameter (NO cascade to avoid multiple paths)
            modelBuilder.Entity<TestResult>()
                .HasOne(r => r.TestParameter)
                .WithMany()
                .HasForeignKey(r => r.TestParameterId)
                .OnDelete(DeleteBehavior.Restrict); // or SetNull if FK is nullable


            // If you want Balance persisted in DB as a computed column, uncomment below
            // and change Balance to have a private setter in the entity:
            // modelBuilder.Entity<Invoice>()
            //     .Property(i => i.Balance)
            //     .HasComputedColumnSql("[TotalAmount] - [PaidAmount]", stored: true);

            // ===== Seed Data =====

            modelBuilder.Entity<LabTest>().HasData(
                new LabTest { Id = 1, Code = "CBC01", Name = "CBC", Category = "Hematology", Price = 300 },
                new LabTest { Id = 2, Code = "KFT01", Name = "KFT", Category = "Biochemistry", Price = 500 },
                new LabTest { Id = 3, Code = "LFT01", Name = "LFT", Category = "Biochemistry", Price = 600 },
                new LabTest { Id = 4, Code = "LIP01", Name = "Lipid Profile", Category = "Biochemistry", Price = 700 }
            );


            modelBuilder.Entity<TestParameter>().HasData(
      new TestParameter { Id = 1, LabTestId = 1, ParameterName = "Hemoglobin", Unit = "g/dL", ReferenceRange = "12-16", Definition = "Hemoglobin measures oxygen-carrying protein in red blood cells; low levels may indicate anemia." },
      new TestParameter { Id = 2, LabTestId = 1, ParameterName = "WBC Count", Unit = "/mm³", ReferenceRange = "4000-11000", Definition = "White blood cell count indicates immune system activity; high levels suggest infection or inflammation." },
      new TestParameter { Id = 3, LabTestId = 1, ParameterName = "RBC Count", Unit = "mill/µL", ReferenceRange = "4.5-5.9", Definition = "Red blood cell count measures number of red cells; abnormalities may indicate anemia or polycythemia." },
      new TestParameter { Id = 4, LabTestId = 1, ParameterName = "Platelets", Unit = "lakh/µL", ReferenceRange = "1.5-4.5", Definition = "Platelets help in blood clotting; low levels increase bleeding risk, high levels increase clot risk." },
      new TestParameter { Id = 5, LabTestId = 1, ParameterName = "PCV (Hematocrit)", Unit = "%", ReferenceRange = "36-46", Definition = "Hematocrit measures proportion of blood occupied by red cells; low values suggest anemia." },

      // 🔹 RBC Indices
      new TestParameter { Id = 6, LabTestId = 1, ParameterName = "MCV (Mean Corpuscular Volume)", Unit = "fL", ReferenceRange = "80-100", Definition = "MCV indicates average size of red blood cells; useful for anemia classification." },
      new TestParameter { Id = 7, LabTestId = 1, ParameterName = "MCH (Mean Corpuscular Hemoglobin)", Unit = "pg", ReferenceRange = "27-32", Definition = "MCH shows average hemoglobin per red cell; helps identify anemia type." },
      new TestParameter { Id = 8, LabTestId = 1, ParameterName = "MCHC (Mean Corpuscular Hemoglobin Concentration)", Unit = "g/dL", ReferenceRange = "32-36", Definition = "MCHC indicates hemoglobin concentration within red cells; abnormal in anemia." },
      new TestParameter { Id = 9, LabTestId = 1, ParameterName = "RDW (Red Cell Distribution Width)", Unit = "%", ReferenceRange = "11-15", Definition = "RDW measures variation in red cell size; high values suggest mixed anemia." },

      // 🔹 Differential WBC Count
      new TestParameter { Id = 10, LabTestId = 1, ParameterName = "Neutrophils", Unit = "%", ReferenceRange = "40-60", Definition = "Neutrophils are first-line defense against infection; high in bacterial infections." },
      new TestParameter { Id = 11, LabTestId = 1, ParameterName = "Lymphocytes", Unit = "%", ReferenceRange = "20-40", Definition = "Lymphocytes are key for immunity; high in viral infections." },
      new TestParameter { Id = 12, LabTestId = 1, ParameterName = "Monocytes", Unit = "%", ReferenceRange = "2-8", Definition = "Monocytes fight chronic infection and inflammation." },
      new TestParameter { Id = 13, LabTestId = 1, ParameterName = "Eosinophils", Unit = "%", ReferenceRange = "1-6", Definition = "Eosinophils increase in allergies and parasitic infections." },
      new TestParameter { Id = 14, LabTestId = 1, ParameterName = "Basophils", Unit = "%", ReferenceRange = "0-1", Definition = "Basophils release histamine in allergic reactions." },

      // 🔹 Absolute WBC Counts
      new TestParameter { Id = 15, LabTestId = 1, ParameterName = "Absolute Neutrophil Count", Unit = "/mm³", ReferenceRange = "1500-8000", Definition = "Absolute neutrophil count evaluates infection-fighting capacity." },
      new TestParameter { Id = 16, LabTestId = 1, ParameterName = "Absolute Lymphocyte Count", Unit = "/mm³", ReferenceRange = "1000-4800", Definition = "Absolute lymphocyte count reflects immune strength." },
      new TestParameter { Id = 17, LabTestId = 1, ParameterName = "Absolute Monocyte Count", Unit = "/mm³", ReferenceRange = "200-800", Definition = "Absolute monocyte count helps assess chronic infection or inflammation." },
      new TestParameter { Id = 18, LabTestId = 1, ParameterName = "Absolute Eosinophil Count", Unit = "/mm³", ReferenceRange = "50-500", Definition = "Absolute eosinophil count increases in allergies and asthma." },
      new TestParameter { Id = 19, LabTestId = 1, ParameterName = "Absolute Basophil Count", Unit = "/mm³", ReferenceRange = "0-200", Definition = "Absolute basophil count is rarely elevated; linked to allergic responses." }
  );





            // KFT
            modelBuilder.Entity<TestParameter>().HasData(
     new TestParameter { Id = 20, LabTestId = 2, ParameterName = "Blood Urea", Unit = "mg/dL", ReferenceRange = "15-40", Definition = "Blood Urea indicates kidney’s ability to remove waste products." },
     new TestParameter { Id = 21, LabTestId = 2, ParameterName = "Serum Creatinine", Unit = "mg/dL", ReferenceRange = "0.6-1.2", Definition = "Creatinine level helps assess kidney filtration efficiency." },
     new TestParameter { Id = 22, LabTestId = 2, ParameterName = "Uric Acid", Unit = "mg/dL", ReferenceRange = "Male: 3.4-7.0, Female: 2.4-6.0", Definition = "Uric acid measures purine metabolism; high levels may cause gout or kidney issues." },
     new TestParameter { Id = 23, LabTestId = 2, ParameterName = "Blood Urea Nitrogen (BUN)", Unit = "mg/dL", ReferenceRange = "7-20", Definition = "BUN reflects nitrogen waste in blood; used for kidney function testing." },
     new TestParameter { Id = 24, LabTestId = 2, ParameterName = "BUN/Creatinine Ratio", Unit = "Ratio", ReferenceRange = "10:1 – 20:1", Definition = "Ratio helps differentiate between kidney and non-kidney causes of dysfunction." },
     new TestParameter { Id = 25, LabTestId = 2, ParameterName = "Sodium (Na⁺)", Unit = "mmol/L", ReferenceRange = "135-145", Definition = "Sodium is essential for fluid balance and nerve function." },
     new TestParameter { Id = 26, LabTestId = 2, ParameterName = "Potassium (K⁺)", Unit = "mmol/L", ReferenceRange = "3.5-5.0", Definition = "Potassium is important for heart and muscle function." },
     new TestParameter { Id = 27, LabTestId = 2, ParameterName = "Chloride (Cl⁻)", Unit = "mmol/L", ReferenceRange = "96-106", Definition = "Chloride helps maintain acid-base balance and hydration." },
     new TestParameter { Id = 28, LabTestId = 2, ParameterName = "Calcium (Ca²⁺)", Unit = "mg/dL", ReferenceRange = "8.5-10.5", Definition = "Calcium is needed for bones, nerves, and muscles." },
     new TestParameter { Id = 29, LabTestId = 2, ParameterName = "Phosphorus", Unit = "mg/dL", ReferenceRange = "2.5-4.5", Definition = "Phosphorus is essential for energy production and bone health." },
     new TestParameter { Id = 30, LabTestId = 2, ParameterName = "eGFR", Unit = "mL/min/1.73m²", ReferenceRange = ">90", Definition = "eGFR estimates kidney filtration rate; key marker for CKD." }
 );

            // LFT
            modelBuilder.Entity<TestParameter>().HasData(
      new TestParameter { Id = 31, LabTestId = 3, ParameterName = "Total Bilirubin", Unit = "mg/dL", ReferenceRange = "0.3-1.2", Definition = "Total bilirubin measures breakdown of red blood cells; high levels indicate jaundice." },
      new TestParameter { Id = 32, LabTestId = 3, ParameterName = "Direct Bilirubin", Unit = "mg/dL", ReferenceRange = "0.0-0.3", Definition = "Direct bilirubin indicates liver’s ability to excrete bile." },
      new TestParameter { Id = 33, LabTestId = 3, ParameterName = "Indirect Bilirubin", Unit = "mg/dL", ReferenceRange = "0.2-0.9", Definition = "Indirect bilirubin is unconjugated; imbalance may indicate hemolysis." },
      new TestParameter { Id = 34, LabTestId = 3, ParameterName = "SGOT (AST)", Unit = "U/L", ReferenceRange = "Male: <40, Female: <35", Definition = "AST enzyme helps evaluate liver and heart health." },
      new TestParameter { Id = 35, LabTestId = 3, ParameterName = "SGPT (ALT)", Unit = "U/L", ReferenceRange = "Male: <40, Female: <35", Definition = "ALT enzyme is specific for liver injury." },
      new TestParameter { Id = 36, LabTestId = 3, ParameterName = "Alkaline Phosphatase (ALP)", Unit = "U/L", ReferenceRange = "44-147", Definition = "ALP helps detect liver, bile duct, and bone disorders." },
      new TestParameter { Id = 37, LabTestId = 3, ParameterName = "Gamma GT (GGT)", Unit = "U/L", ReferenceRange = "Male: 15-85, Female: 5-55", Definition = "GGT is elevated in alcohol or bile duct-related liver diseases." },
      new TestParameter { Id = 38, LabTestId = 3, ParameterName = "Total Protein", Unit = "g/dL", ReferenceRange = "6.0-8.3", Definition = "Total protein indicates nutritional status and liver function." },
      new TestParameter { Id = 39, LabTestId = 3, ParameterName = "Albumin", Unit = "g/dL", ReferenceRange = "3.5-5.0", Definition = "Albumin maintains osmotic pressure and transports substances." },
      new TestParameter { Id = 40, LabTestId = 3, ParameterName = "Globulin", Unit = "g/dL", ReferenceRange = "2.0-3.5", Definition = "Globulins play roles in immunity and transport." },
      new TestParameter { Id = 41, LabTestId = 3, ParameterName = "A/G Ratio", Unit = "Ratio", ReferenceRange = "1.2-2.2", Definition = "Albumin/Globulin ratio helps in diagnosing liver and kidney disease." }
  );

            // Lipid Profile
            modelBuilder.Entity<TestParameter>().HasData(
     new TestParameter { Id = 42, LabTestId = 4, ParameterName = "Total Cholesterol", Unit = "mg/dL", ReferenceRange = "<200", Definition = "Total cholesterol measures overall fat in the blood; high levels increase heart risk." },
     new TestParameter { Id = 43, LabTestId = 4, ParameterName = "Triglycerides", Unit = "mg/dL", ReferenceRange = "<150", Definition = "Triglycerides are stored fats; high levels increase risk of heart disease." },
     new TestParameter { Id = 44, LabTestId = 4, ParameterName = "HDL Cholesterol", Unit = "mg/dL", ReferenceRange = "Male: >40, Female: >50", Definition = "HDL is ‘good’ cholesterol that protects against heart disease." },
     new TestParameter { Id = 45, LabTestId = 4, ParameterName = "LDL Cholesterol", Unit = "mg/dL", ReferenceRange = "<100 Optimal", Definition = "LDL is ‘bad’ cholesterol; high levels lead to artery blockage." },
     new TestParameter { Id = 46, LabTestId = 4, ParameterName = "VLDL Cholesterol", Unit = "mg/dL", ReferenceRange = "5-30", Definition = "VLDL carries triglycerides; high levels indicate high fat in blood." },
     new TestParameter { Id = 47, LabTestId = 4, ParameterName = "Non-HDL Cholesterol", Unit = "mg/dL", ReferenceRange = "<130", Definition = "Non-HDL cholesterol is a better predictor of heart risk than LDL alone." },
     new TestParameter { Id = 48, LabTestId = 4, ParameterName = "TC/HDL Ratio", Unit = "Ratio", ReferenceRange = "<5", Definition = "Cholesterol/HDL ratio helps evaluate heart risk." },
     new TestParameter { Id = 49, LabTestId = 4, ParameterName = "LDL/HDL Ratio", Unit = "Ratio", ReferenceRange = "<3.5", Definition = "LDL/HDL ratio is used to assess risk of cardiovascular disease." }
 );
        }
    }
}
