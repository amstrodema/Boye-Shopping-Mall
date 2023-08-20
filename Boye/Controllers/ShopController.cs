using Boye.ModelHybrids;
using Boye.Models;
using Boye.ModelViews;
using Boye.Repository;
using Boye.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Net;
using System.Text;

namespace Boye.Controllers
{
    public class ShopController : Controller
    {
        private readonly BoyeRepository _db;
        private readonly IHttpContextAccessor _context;

        public ShopController(BoyeRepository db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _context = httpContextAccessor;
        }
        public IActionResult Index(string id)
        {
            ShopVM shopVM = new ShopVM();
            var items =  _db.Items.Where(i => i.IsActive).ToList();
            shopVM.Categories = _db.Categories.Where(c => c.IsActive).ToList();
            ViewBag.Category = "All";

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    try
                    {
                        Guid catID = Guid.Parse(id);
                        var category = _db.Categories.FirstOrDefault(c => c.ID == catID && c.IsActive);
                        if (category != default)
                        {
                            items = items.Where(p => p.CatID == catID && p.IsActive).ToList();
                            ViewBag.Category = category.Name;
                        }                        
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                shopVM.Items = (from itm in items
                                      select new ItemHybrid()
                                      {
                                          Image = ImageService.GetImageFromFolder(itm.Image, "Item-Main"),
                                          Name = itm.Name,
                                          Desc = itm.Desc,
                                          Price = itm.Price,
                                          ID = itm.ID
                                      }).ToList();
            }
            catch (Exception)
            {
            }

            return View(shopVM);
        }
        public IActionResult Cart()
        {
            List<CartHolder> cart = new List<CartHolder>();
            try
            {
                var cartString = _context.HttpContext.Session.GetString("cart");
                cart = JsonConvert.DeserializeObject<List<CartHolder>>(cartString);

                cart = (from crt in cart
                        join item in _db.Items on crt.ID equals item.ID
                        select new CartHolder()
                        {
                            ID = crt.ID,
                            Item = item.Name,
                            Price = item.Price,
                            Image = ImageService.GetSmallImageFromFolder(item.Image, "Item-Main"),
                            Qty = crt.Qty,
                            Cost = item.Price * crt.Qty
                        }).ToList();
            }
            catch (Exception)
            {
                cart = new List<CartHolder>();
            }
            return View(cart);
        }
        [HttpPost]
        public IActionResult AddToCart([FromBody]string id)
        {
            Guid cartID;

            try
            {
                cartID = Guid.Parse(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            try
            {
                var cartString = _context.HttpContext.Session.GetString("cart");
                var cart = JsonConvert.DeserializeObject<List<CartHolder>>(cartString);
                var cartItem = cart.FirstOrDefault(c=> c.ID == cartID);

                if (cartItem == default)
                {
                    cart.Add(new CartHolder() { ID = cartID, Qty = 1 });
                }
                else
                {
                    cart.Remove(cartItem);

                    cartItem.Qty++;
                    cart.Add(cartItem);
                }
                _context.HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            catch (Exception)
            {
                List<CartHolder> cart = new List<CartHolder>
                    {
                        new CartHolder() { ID = cartID, Qty = 1 }
                    };
                _context.HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
           // 
           // //throw new Exception();
           // //return View("Cart");
            return Ok();
        }
        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] string id) {
            if (RemoveCartItem(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        public bool RemoveCartItem(string id)
        {
            Guid cartID;

            try
            {
                cartID = Guid.Parse(id);
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                var cartString = _context.HttpContext.Session.GetString("cart");
                var cart = JsonConvert.DeserializeObject<List<CartHolder>>(cartString);
                var cartItem = cart.FirstOrDefault(c=> c.ID == cartID);

                if (cartItem == default)
                {
                    ViewBag.Alert = "Item not found!";
                    return true;
                }
                else
                {
                    cart.Remove(cartItem);

                    cartItem.Qty--;
                    ViewBag.Alert = "Item Removed";

                    if (cartItem.Qty > 0)
                    {
                        cart.Add(cartItem);
                        ViewBag.Alert = "Item quantity = " + cartItem.Qty;
                    }

                    _context.HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                    return true;
                }
              
            }
            catch (Exception)
            {
                ViewBag.Alert = "Cart is empty!";
                return true;
            }
           // 
           // //throw new Exception();
           // //return View("Cart");
            
        }
        public IActionResult CheckCart()
        {
            int val = 0;
            try
            {
                var cartString = _context.HttpContext.Session.GetString("cart");
                var cart = JsonConvert.DeserializeObject<List<CartHolder>>(cartString);
                val = cart.Count();
            }
            catch (Exception)
            {
            }
            return Ok(val);
        }
        public IActionResult CheckOut()
        {
            List<CartHolder> cart = new List<CartHolder>();
            try
            {
                var cartString = _context.HttpContext.Session.GetString("cart");
                cart = JsonConvert.DeserializeObject<List<CartHolder>>(cartString);

                cart = (from crt in cart
                        join item in _db.Items on crt.ID equals item.ID
                        select new CartHolder()
                        {
                            ID = crt.ID,
                            Item = item.Name,
                            Price = item.Price,
                            Image = ImageService.GetSmallImageFromFolder(item.Image, "Item-Main"),
                            Qty = crt.Qty,
                            Cost = item.Price * crt.Qty
                        }).ToList();
            }
            catch (Exception)
            {
                return RedirectToAction("Cart");
            }

            CartVM vm = new CartVM();
            vm.Cart = cart;

            return View(vm);
        }
        public IActionResult Order()
        {
            try
            {
                var cartString = _context.HttpContext.Session.GetString("cart");
                var cart = JsonConvert.DeserializeObject<List<CartHolder>>(cartString);
                //Order

                foreach (var item in cart)
                {

                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        public IActionResult Item(string id)
        {
            Guid itemID;
            try
            {
                 itemID = Guid.Parse(id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            var val = _db.Items.FirstOrDefault(i => i.ID == itemID);
            if (val == default)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var cartString = _context.HttpContext.Session.GetString("cart");
                var cart = JsonConvert.DeserializeObject<List<CartHolder>>(cartString);
                var cartItem = cart.FirstOrDefault(c => c.ID == val.ID);

                if (cartItem == default)
                {
                    val.Qty = 0;
                }
                else
                {
                    val.Qty = cartItem.Qty;
                }
            }
            catch (Exception)
            {
                val.Qty = 0;
            }

            val.Image = ImageService.GetImageFromFolder(val.Image, "Item-Main");

            return View(val);
        }

        //public IActionResult ProcessOrder() {

        //    return View();
        //}

        [HttpPost]
        public IActionResult ProcessOrder(CartVM cartVM)
        {
            try
            {
                string checkOutOrder = "*BOYE SHOPPING MALL*%0A_CheckOut Order_%0A%0A";
                Order order = cartVM.Order;
                order.ID = Guid.NewGuid();
                order.DateCreated = DateTime.Now;

                var cartString = _context.HttpContext.Session.GetString("cart");
                var cart = JsonConvert.DeserializeObject<List<CartHolder>>(cartString);

                if (cart.Count() < 1 || string.IsNullOrEmpty(order.BuyerPhone))
                {
                    throw new Exception();
                }

                IEnumerable<OrderItem> orderItems = from cartItem in cart
                                                    join item in _db.Items on cartItem.ID equals item.ID
                                                    join categ in _db.Categories on item.CatID equals categ.ID
                                                    select new OrderItem()
                                                    {
                                                        ID = Guid.NewGuid(),
                                                        ItemID = cartItem.ID,
                                                        Qty = cartItem.Qty,
                                                        Category = categ.Name,
                                                        Name = item.Name,
                                                        Desc = item.Desc,
                                                        Image = item.Image,
                                                        Price = item.Price,
                                                        OrderID = order.ID,
                                                        Cost = item.Price * cartItem.Qty
                                                    };

                foreach (var cartItem in orderItems)
                {
                    checkOutOrder += $"{cartItem.Name}:%20%20x{cartItem.Qty.ToString("#.##")}%20%20=%20%20₦{cartItem.Cost.ToString("#.##")}%0A";
                }
                checkOutOrder += $"*TOTAL: ₦{orderItems.Sum(s=> s.Cost).ToString("#.##")}*%0A";
                checkOutOrder += $"%0A*Customer Details*%0AName: {order.BuyerName}%0APhone: {order.BuyerPhone}%0AAddress:  {order.BuyerAddress}";
                checkOutOrder += $"%0A%0A%0A*Order Created*%0A{order.DateCreated.ToString("F")}";

                FileService.UpdateFile(orderItems.ToJson().Substring(1,orderItems.Count()-2), "OrderItems");
                return View("ProcessOrder", checkOutOrder);
                // await _db.OrderItems.AddRangeAsync(orderItems.ToArray());
                //await _db.Orders.AddAsync(order);

                // if (await _db.SaveChangesAsync() > 0)
                // {
                //     return View("ProcessOrder", checkOutOrder);
                // }
                // else
                // {
                //     throw new Exception();
                // }
            }
            catch (Exception)
            {
                return RedirectToAction("Cart");
            }
            
        }
        public IActionResult ClearCart()
        {
            _context.HttpContext.Session.Remove("cart");
            return RedirectToAction("Index");
        }
        public IActionResult RemoveItem(string id)
        {
            RemoveCartItem(id);
            return RedirectToAction("Cart");
        }
      
        }
}
