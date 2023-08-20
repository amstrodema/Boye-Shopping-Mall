namespace Boye.Models
{
    public class Item
    {
        public Guid ID { get; set; }
        public Guid CatID { get; set; }
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }

        public bool IsNewArrival { get; set; }
        public bool IsPopular { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
