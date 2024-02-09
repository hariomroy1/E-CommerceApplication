using System.ComponentModel.DataAnnotations;

namespace Training.Order.Model
{
    public class OrderEntity
    {
        [Key]
        [Required]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int RegisterId { get; set; }
        public DateTime OrderDate { get; set; }
        public int QuantityOfItems { get; set; }
        public int TotalPrice { get; set; }
    }
}
