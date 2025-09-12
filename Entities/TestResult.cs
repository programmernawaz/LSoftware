using System.ComponentModel.DataAnnotations;

namespace PathLabAPI.Entities
{
    public class TestResult
    {
        [Key]
        public int Id { get; set; }

        public int TestOrderItemId { get; set; }
        public int TestParameterId { get; set; }

        public string? Value { get; set; }  // technician input
        public string? Flag { get; set; }   // L, H, N

        public TestOrderItem TestOrderItem { get; set; } = null!;
        public TestParameter TestParameter { get; set; } = null!;
        public string? Notes { get; internal set; }
    }
}
