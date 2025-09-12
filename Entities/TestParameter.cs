using System.ComponentModel.DataAnnotations;

namespace PathLabAPI.Entities
{
    public class TestParameter
    {
        [Key]
        public int Id { get; set; }
        public int LabTestId { get; set; }
        public string ParameterName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string ReferenceRange { get; set; } = string.Empty;
        public double? MinValue { get; set; }     // numeric low bound (if applicable)
        public double? MaxValue { get; set; }
        public string Definition { get; set; } = string.Empty;

        public LabTest LabTest { get; set; } = null!;

    }
}
