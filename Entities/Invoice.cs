using System.ComponentModel.DataAnnotations;

namespace PathLabAPI.Entities
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public int TestOrderId { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Balance => TotalAmount - PaidAmount;

        public TestOrder TestOrder { get; set; } = null!;

    }
}
