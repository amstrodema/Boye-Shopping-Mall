using Boye.Models;
using Boye.Services;
using Newtonsoft.Json;

namespace Boye.Repository
{
    public class BoyeRepository
    {
        public List<Category> Categories { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Item> Items { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<User> Users { get; set; }

        public BoyeRepository()
        {
            string str = FileService.ReadFromFile("Category") == null ? "" : FileService.ReadFromFile("Categories");
            var categories = JsonConvert.DeserializeObject<List<Category>>(str);
            str = FileService.ReadFromFile("Contact") == null ? "" : FileService.ReadFromFile("Contacts");
            var contacts = JsonConvert.DeserializeObject<List<Contact>>(str);
            str = FileService.ReadFromFile("Item") == null ? "" : FileService.ReadFromFile("Items");
            var items = JsonConvert.DeserializeObject<List<Item>>(str);
            str = FileService.ReadFromFile("Order") == null ? "" : FileService.ReadFromFile("Orders");
            var orders = JsonConvert.DeserializeObject<List<Order>>(str);
            str = FileService.ReadFromFile("OrderItem") == null ? "" : FileService.ReadFromFile("OrderItems");
            var orderItems = JsonConvert.DeserializeObject<List<OrderItem>>(str);
            str = FileService.ReadFromFile("Picture") == null ? "" : FileService.ReadFromFile("Pictures");
            var pictures = JsonConvert.DeserializeObject<List<Picture>>(str);
            str = FileService.ReadFromFile("User") == null ? "" : FileService.ReadFromFile("Users");
            var users = JsonConvert.DeserializeObject<List<User>>(str);

            Categories = categories == null ? new List<Category>() : categories;
            Contacts = contacts == null ? new List<Contact>() : contacts;
            Items = items == null ? new List<Item>() : items;
            Users = users == null ? new List<User>() : users;
            Orders = orders == null ? new List<Order>() : orders;
            OrderItems = orderItems == null ? new List<OrderItem>() : orderItems;
            Pictures = pictures == null ? new List<Picture>() : pictures;
        }
    }
}
