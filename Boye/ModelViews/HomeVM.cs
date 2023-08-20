using Boye.Models;

namespace Boye.ModelViews
{
    public class HomeVM
    {
        public IEnumerable<Item> NewArrivals { get; set; } = new List<Item>();
        public IEnumerable<Item> PopularItems { get; set; } = new List<Item>();
    }
}
