using System.ComponentModel.DataAnnotations;

namespace PathLabAPI.Entities
{
    public class TestOrderItem
    {
        [Key]
        public int Id { get; set; }
        public int TestOrderId { get; set; }
        public int LabTestId { get; set; }

        public TestOrder TestOrder { get; set; } = null!;
        public LabTest LabTest { get; set; } = null!;

        public ICollection<TestResult> Results { get; set; } = new List<TestResult>();

    }
}
