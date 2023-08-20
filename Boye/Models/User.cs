namespace Boye.Models
{
    public class User
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsDev { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
