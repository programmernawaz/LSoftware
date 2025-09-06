using System.ComponentModel.DataAnnotations;

namespace PathLabAPI.Entities
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;

        public ICollection<TestOrder> Orders { get; set; } = new List<TestOrder>();

    }
}
