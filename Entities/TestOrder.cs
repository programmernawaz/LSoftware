using System.ComponentModel.DataAnnotations;

namespace PathLabAPI.Entities
{
    public class TestOrder
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public Patient Patient { get; set; } = null!;
        public Doctor? Doctor { get; set; }
        public ICollection<TestOrderItem> Items { get; set; } = new List<TestOrderItem>();
        public Invoice? Invoice { get; set; }

    }
}
