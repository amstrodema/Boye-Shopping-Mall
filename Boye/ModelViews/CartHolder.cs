namespace Boye.ModelViews
{
    public class CartHolder
    {
        public Guid ID { get; set; }
        public int Qty { get; set; }
        public string? Item { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
    }
}
