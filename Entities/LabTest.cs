using System.ComponentModel.DataAnnotations;

namespace PathLabAPI.Entities
{
    public class LabTest
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;   // CBC01
        public string Name { get; set; } = string.Empty;   // CBC, KFT, etc.
        public string Category { get; set; } = string.Empty; // Hematology, Biochemistry
        public decimal Price { get; set; }

        public ICollection<TestParameter> Parameters { get; set; } = new List<TestParameter>();

    }
}
