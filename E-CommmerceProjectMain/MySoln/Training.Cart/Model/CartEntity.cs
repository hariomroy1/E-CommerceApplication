using System.ComponentModel.DataAnnotations;

namespace Training.Cart.Model
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
