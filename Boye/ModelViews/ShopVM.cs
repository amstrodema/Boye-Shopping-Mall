using Boye.ModelHybrids;
using Boye.Models;

namespace Boye.ModelViews
{
    public class ShopVM
    {
        public IEnumerable<ItemHybrid> Items { get; set; } = new List<ItemHybrid>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
