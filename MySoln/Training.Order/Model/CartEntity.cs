using System.ComponentModel.DataAnnotations;
namespace Training.Order.Model
{
    public class CartEntity
    {
        [Key]
        [Required]
        public int CartId { get; set; }
        public int RegisterId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
