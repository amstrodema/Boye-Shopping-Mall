﻿namespace Boye.Models
{
    public class Category
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Desc { get; set; }

        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
