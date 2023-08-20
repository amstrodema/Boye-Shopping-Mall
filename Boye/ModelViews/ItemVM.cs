using Boye.Models;

namespace Boye.ModelViews
{
    public class ItemVM
    {
        public Item Item { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int CategMapping { get; set; }
    }
}
