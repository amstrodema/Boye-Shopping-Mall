using Boye.Models;

namespace Boye.ModelHybrids
{
    public class OrderHybrid
    {
        public Guid ID { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerEmail { get; set; }
        public string? BuyerPhone { get; set; }
        public string? BuyerAddress { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
