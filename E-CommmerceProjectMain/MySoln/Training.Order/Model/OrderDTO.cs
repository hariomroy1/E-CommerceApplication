namespace Training.Order.Model
{
    public class OrderDTO
    {
        public int RegisterId { get; set; }

        public int productId { get; set; }
        public int QuantityOfItems { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public int[] UpdatedQuantities { get; set; }
    }
}
