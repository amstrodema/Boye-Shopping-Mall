namespace Boye.ModelHybrids
{
    public class ItemHybrid
    {
        public Guid ID { get; set; }
        public string? Categ { get; set; }
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
