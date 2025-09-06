namespace PathLabAPI.Dto
{
    public class LabTestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ParameterDto> Parameters { get; set; } = new();
    }

    public class ParameterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string ReferenceRange { get; set; } = string.Empty;
    }

    public class TestOrderDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        public DateTime OrderDate { get; set; }

        public PatientDto? Patient { get; set; }          // nullable nav
        public DoctorDto? Doctor { get; set; }            // nullable nav
        public List<TestOrderItemDto> Items { get; set; } = new(); // always init lists
        public InvoiceDto? Invoice { get; set; }          // nullable nav
    }

    public class PatientDto
    {
        public int Id { get; set; }
        public string PatientCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class DoctorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
    }

    public class TestOrderItemDto
    {
        public int Id { get; set; }
        public int LabTestId { get; set; }
        public LabTestDto? LabTest { get; set; }          // nullable nav
    }

    public class InvoiceDto
    {
        public int Id { get; set; }
        public int TestOrderId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }

        // Derived to avoid inconsistency
       // public decimal Balance => TotalAmount - PaidAmount;
    }
}
