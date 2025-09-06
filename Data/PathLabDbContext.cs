using Microsoft.EntityFrameworkCore;
using PathLabAPI.Entities;

namespace PathLabAPI.Data
{
    public class PathLabDbContext : DbContext
    {
        public PathLabDbContext(DbContextOptions<PathLabDbContext> options) : base(options) { }

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

            // CBC
            modelBuilder.Entity<TestParameter>().HasData(
                new TestParameter { Id = 1, LabTestId = 1, ParameterName = "Hemoglobin", Unit = "g/dL", ReferenceRange = "12-16" },
                new TestParameter { Id = 2, LabTestId = 1, ParameterName = "WBC Count", Unit = "/mm³", ReferenceRange = "4000-11000" },
                new TestParameter { Id = 3, LabTestId = 1, ParameterName = "RBC Count", Unit = "mill/µL", ReferenceRange = "4.5-5.9" },
                new TestParameter { Id = 4, LabTestId = 1, ParameterName = "Platelets", Unit = "lakh/µL", ReferenceRange = "1.5-4.5" },
                new TestParameter { Id = 5, LabTestId = 1, ParameterName = "PCV (Hematocrit)", Unit = "%", ReferenceRange = "36-46" }
            );

            // KFT
            modelBuilder.Entity<TestParameter>().HasData(
                new TestParameter { Id = 6, LabTestId = 2, ParameterName = "Urea", Unit = "mg/dL", ReferenceRange = "15-40" },
                new TestParameter { Id = 7, LabTestId = 2, ParameterName = "Creatinine", Unit = "mg/dL", ReferenceRange = "0.6-1.3" },
                new TestParameter { Id = 8, LabTestId = 2, ParameterName = "Sodium (Na)", Unit = "mEq/L", ReferenceRange = "135-145" },
                new TestParameter { Id = 9, LabTestId = 2, ParameterName = "Potassium (K)", Unit = "mEq/L", ReferenceRange = "3.5-5.0" }
            );

            // LFT
            modelBuilder.Entity<TestParameter>().HasData(
                new TestParameter { Id = 10, LabTestId = 3, ParameterName = "Total Bilirubin", Unit = "mg/dL", ReferenceRange = "0.3-1.2" },
                new TestParameter { Id = 11, LabTestId = 3, ParameterName = "Direct Bilirubin", Unit = "mg/dL", ReferenceRange = "0.0-0.3" },
                new TestParameter { Id = 12, LabTestId = 3, ParameterName = "SGOT (AST)", Unit = "U/L", ReferenceRange = "5-40" },
                new TestParameter { Id = 13, LabTestId = 3, ParameterName = "SGPT (ALT)", Unit = "U/L", ReferenceRange = "5-35" },
                new TestParameter { Id = 14, LabTestId = 3, ParameterName = "Alkaline Phosphatase", Unit = "U/L", ReferenceRange = "44-147" },
                new TestParameter { Id = 15, LabTestId = 3, ParameterName = "Total Protein", Unit = "g/dL", ReferenceRange = "6.0-8.3" },
                new TestParameter { Id = 16, LabTestId = 3, ParameterName = "Albumin", Unit = "g/dL", ReferenceRange = "3.5-5.0" }
            );

            // Lipid Profile
            modelBuilder.Entity<TestParameter>().HasData(
                new TestParameter { Id = 17, LabTestId = 4, ParameterName = "Total Cholesterol", Unit = "mg/dL", ReferenceRange = "<200" },
                new TestParameter { Id = 18, LabTestId = 4, ParameterName = "Triglycerides", Unit = "mg/dL", ReferenceRange = "<150" },
                new TestParameter { Id = 19, LabTestId = 4, ParameterName = "HDL Cholesterol", Unit = "mg/dL", ReferenceRange = ">40" },
                new TestParameter { Id = 20, LabTestId = 4, ParameterName = "LDL Cholesterol", Unit = "mg/dL", ReferenceRange = "<100" },
                new TestParameter { Id = 21, LabTestId = 4, ParameterName = "VLDL", Unit = "mg/dL", ReferenceRange = "5-40" }
            );
        }
    }
}
