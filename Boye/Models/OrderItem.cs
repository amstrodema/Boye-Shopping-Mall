namespace Boye.Models
{
    public class OrderItem
    {
        public Guid ID { get; set; }
        public Guid ItemID { get; set; }
        public Guid OrderID { get; set; }

        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public string? Image { get; set; }

        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
    }
}
