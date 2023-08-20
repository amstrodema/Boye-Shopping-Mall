using Boye.ModelHybrids;
using Boye.Models;
using Boye.ModelViews;
using Boye.Repository;
using Boye.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Configuration;
using System.IO;

namespace Boye.Controllers
{
    public class ManageController : Controller
    {
        private readonly IHttpContextAccessor _context;
        BoyeRepository _db;
        LoginValidator _loginValidator;

        public ManageController(BoyeRepository db, LoginValidator loginValidator, IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
            _db = db;
            _loginValidator = loginValidator;
        }
        public IActionResult Index()
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }
            return View();
        }
        public IActionResult Categories()
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }
            var categories = _db.Categories.Where(m=> m.IsActive).ToList();
            return View(categories);
        }
        public IActionResult Category()
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Category(string name, string desc)
        {
            try
            {
                if (!_loginValidator.IsLoggedIn())
                {
                    return RedirectToAction("Auth");
                }
                Category category = new Category()
                {
                    ID = Guid.NewGuid(),
                    CreatedBy = Guid.Parse(_loginValidator.GetUserID()),
                    DateCreated = DateTime.Now,
                    Desc = desc,
                    IsActive = true,
                    Name = name
                };

                var check =  _db.Categories.FirstOrDefault(p => p.Name == category.Name && p.Desc == category.Desc && p.IsActive);
                if (check != default)
                {
                    ViewBag.Danger = "Category Repetition!";
                    return View();
                }

                //await _db.Categories.AddAsync(category);
                //if (await _db.SaveChangesAsync() > 0)
                //{
                //    ViewBag.Success = "Category Added!";
                //}
                //else
                //{
                //    ViewBag.Danger = "No Category Added!";
                //}

                ViewBag.Success = "Category Added!";
                FileService.WriteToFile(category.ToJson(), "Categories");
            }
            catch (Exception)
            {
                ViewBag.Danger = "No Category Added!";
            }
            

            return View();
        }
        public IActionResult Items()
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }
            var val = from item in _db.Items
                      where item.IsActive
                      join cat in _db.Categories on item.CatID equals cat.ID
                      select new ItemHybrid()
                      {
                          Categ = cat.Name,
                          DateCreated = item.DateCreated,
                          Desc = item.Desc,
                          ID = item.ID,
                          Image = ImageService.GetSmallImageFromFolder(item.Image, "Item-Main") ,
                          Name = item.Name,
                          Price = item.Price
                      };

            return View(val);
        }
        public IActionResult Item()
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }
            ItemVM itemVM = new ItemVM();
            itemVM.Categories =  _db.Categories.Where(c=> c.IsActive).ToList();
            return View(itemVM);
        }
        [HttpPost]
        public async Task<IActionResult> Item(ItemVM itemVM, IFormFile image)
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }

            try
            {
                Item item = itemVM.Item;

                var check = _db.Items.FirstOrDefault(p => p.Name == item.Name && p.Desc == item.Desc && p.Price == item.Price && p.IsActive );
                if (check != default)
                {
                    ViewBag.Danger = "Item Repetition!"; 
                    
                    itemVM = new ItemVM();
                    itemVM.Categories = _db.Categories.ToList();

                    return View(itemVM);
                }

                item.ID = Guid.NewGuid();
                item.IsActive = true;
                item.DateCreated = DateTime.Now;
                item.CreatedBy = Guid.Parse(_loginValidator.GetUserID());

                if (itemVM.CategMapping == 1)
                {
                    item.IsNewArrival = true;
                    item.IsPopular = false;
                }
                else if (itemVM.CategMapping == 2)
                {
                    item.IsNewArrival = false;
                    item.IsPopular = true;
                }
                else
                {
                    item.IsNewArrival = false;
                    item.IsPopular = false;
                }

                var img = await ImageService.GetByte(image);
                item.Image = ImageService.SaveImageInFolder(img, item.ID.ToString(), "Item-Main");

                //await _db.Items.AddAsync(item);

                //if (await _db.SaveChangesAsync() > 0)
                //{
                //}
                //else
                //{
                //    ViewBag.Danger = "No Item Was Added!";
                //}

               ViewBag.Success = "New Item Added!";
                FileService.WriteToFile(item.ToJson(), "Items");
            }
            catch (Exception)
            {
                ViewBag.Danger = "No Item Was Added!";
            }

            itemVM = new ItemVM();
            itemVM.Categories = _db.Categories.ToList();

            return View(itemVM);
        }
        public IActionResult Orders()
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }
            var val = from order in _db.Orders.Where(o => o.IsActive)
                      join item in _db.Items on order.ID equals item.ID
                      join orderItm in _db.OrderItems on order.ID equals orderItm.OrderID into theOrders
                      select new OrderHybrid()
                      {
                          ID = order.ID,
                          BuyerEmail = order.BuyerEmail,
                          BuyerPhone = order.BuyerPhone,
                          BuyerName = order.BuyerName,
                          DateCreated = order.DateCreated,
                          IsActive = order.IsActive,
                          OrderItems = SortImages(theOrders.ToList()),
                          BuyerAddress = order.BuyerAddress
                      };
            return View(val);
        }
        private IEnumerable<OrderItem> SortImages(List<OrderItem> orderItems)
        {
            for (int i = 0; i < orderItems.Count(); i++)
            {
                orderItems[i].Image = ImageService.GetSmallImageFromFolder(orderItems[i].Image, "Item-Main");
            }

            return orderItems;
        }
        public IActionResult Contacts()
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }
            var val = _db.Contacts.ToList();
            return View(val);
        }

        public IActionResult Auth()
        {
            if (_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Index");
            }

            var user = _db.Users.FirstOrDefault(p => p.Username == "darlos");
            if (user == default)
            {
                User newDev = new User()
                {
                    ID = Guid.NewGuid(),
                    Username = "darlos",
                    IsDev = true,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    Password = EncryptionService.Encrypt("darlos123456")
                };

                User newUser = new User()
                {
                    ID = Guid.NewGuid(),
                    Username = "boyeadmin",
                    IsAdmin = true,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    Password = EncryptionService.Encrypt("admin12345")
                };

                FileService.WriteToFile(newDev.ToJson(), "Users");
                FileService.WriteToFile(newUser.ToJson(), "Users");
               
                //_db.Users.Add(newUser);

                //await _db.SaveChangesAsync();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Auth(string userNameOrEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(userNameOrEmail) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Message = "Invalid Credentials!";
                return View();
            }

            var user = _db.Users.FirstOrDefault(p => p.Username == userNameOrEmail);

            if (user == default)
            {
                ViewBag.Message = "Invalid Credentials!";
                return View();
            }

            if (!EncryptionService.Validate(password, user.Password))
            {
                ViewBag.Message = "Invalid Credentials!";
                return View();
            }

           if (user.IsAdmin)
            {
                _context.HttpContext.Session.SetString("isAdmin", "true");
                _context.HttpContext.Session.SetString("userID", user.ID.ToString());
                _context.HttpContext.Session.SetString("userName", user.Username);
                return RedirectToAction("Index");
            }
         
            else if (user.IsDev)
            {
                _context.HttpContext.Session.SetString("isDev", "true");
                _context.HttpContext.Session.SetString("userID", user.ID.ToString());
                _context.HttpContext.Session.SetString("userName", user.Username);
                return RedirectToAction("Index");
            }

            _context.HttpContext.Session.Clear();
            ViewBag.Message = "Login Failed!";
            return View();
        }
        public async Task<IActionResult> ItemDelete(Guid id)
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }

            try
            {
                var val = _db.Items.FirstOrDefault(i => i.ID == id);
                if (val != default)
                {
                    _db.Items.Remove(val);
                    val.IsActive = false;
                    _db.Items.Add(val);

                    FileService.UpdateFile(_db.Items.ToJson(), "Items");
                    //_db.Items.Update(val);
                    //await _db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Items");
        }
        public async Task<IActionResult> CategoryDelete(Guid id)
        {
            if (!_loginValidator.IsLoggedIn())
            {
                return RedirectToAction("Auth");
            }

            try
            {
                var val = _db.Categories.FirstOrDefault(i => i.ID == id);
                if (val != default)
                {
                    _db.Categories.Remove(val);
                    val.IsActive = false;
                    _db.Categories.Add(val);

                    FileService.UpdateFile(_db.Categories.ToJson(), "Categories");
                    //_db.Categories.Update(val);
                    //await _db.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Categories");
        }

        public ActionResult LogOut()
        {
            _context.HttpContext.Session.Clear();
            return RedirectToAction("Auth");
        }
    }
}
