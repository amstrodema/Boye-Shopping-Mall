using AspNetCore.SEOHelper.Sitemap;
using Boye.Models;
using Boye.ModelViews;
using Boye.Repository;
using Boye.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Diagnostics;

namespace Boye.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BoyeRepository _db;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, BoyeRepository db, IWebHostEnvironment env)
        {
            _logger = logger;
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {

            HomeVM homeVM = new HomeVM();
            try
            {
                homeVM.NewArrivals = (from itm in _db.Items.Where(p => p.IsActive && p.IsNewArrival)
                                      select new Item()
                                      {
                                          Image = ImageService.GetImageFromFolder(itm.Image, "Item-Main"),
                                          Name = itm.Name,
                                          Desc = itm.Desc,
                                          Price = itm.Price,
                                          ID = itm.ID
                                      }).Take(15).ToList();
                homeVM.PopularItems = (from itm in _db.Items.Where(p => p.IsActive && p.IsPopular)
                                       select new Item()
                                       {
                                           Image = ImageService.GetImageFromFolder(itm.Image, "Item-Main"),
                                           Name = itm.Name,
                                           Desc = itm.Desc,
                                           Price = itm.Price,
                                           ID = itm.ID
                                       }).Take(15).ToList();
            }
            catch (Exception)
            {
            }
            return View(homeVM);
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(Contact contact)
        {
            if (contact != null) {
                contact.ID = Guid.NewGuid();
                contact.DateCreated = DateTime.Now;
                contact.IsActive = true;

                if (_db.Contacts.FirstOrDefault(p=> p.Subject == contact.Subject && p.Email == contact.Email && p.Message == contact.Message && p.Name == contact.Name) != default)
                {
                    ViewBag.Success = "Message Sent Already!";
                    return View();
                }

                //await _db.Contacts.AddAsync(contact);
                //if (await _db.SaveChangesAsync() > 0)
                //{
                //    ViewBag.Success = "Message Sent Successfully!";
                //}
                //else
                //{
                //    ViewBag.Danger = "Message Not Sent!";
                //}

                    ViewBag.Success = "Message Sent Successfully!";
                FileService.UpdateFile(_db.Contacts.ToJson(), "Contacts");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string CreateSitemapInRootDirectory()
        {
            string baseUrl = "https://www.boyeshoppingmall.com.ng/";
            var list = new List<SitemapNode>();
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.9, Url = baseUrl, Frequency = SitemapFrequency.Daily });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = $"{baseUrl}shop", Frequency = SitemapFrequency.Daily });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.7, Url = $"{baseUrl}contact", Frequency = SitemapFrequency.Yearly });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.7, Url = $"{baseUrl}about", Frequency = SitemapFrequency.Yearly });
            foreach (var item in _db.Items)
            {
                list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.4, Url = $"{baseUrl}shop/item/{item.ID}", Frequency = SitemapFrequency.Never });
            }
            new SitemapDocument().CreateSitemapXML(list, _env.ContentRootPath);
            return "sitemap";
        }
    }
}