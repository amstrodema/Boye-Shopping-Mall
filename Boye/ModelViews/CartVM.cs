using Boye.Models;

namespace Boye.ModelViews
{
    public class CartVM
    {
        public IEnumerable<CartHolder> Cart { get; set; }
        public Order Order { get; set; }
    }
}
