using System.ComponentModel.DataAnnotations;

namespace PathLabAPI.Entities
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        public string PatientCode { get; set; } = string.Empty; // P0001
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public ICollection<TestOrder> Orders { get; set; } = new List<TestOrder>();

    }
}
