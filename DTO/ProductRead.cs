using System;

namespace SoftwareFullComponents.LicenseComponent.DTO
{
    public class ProductRead
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string ProductName { get; set; }
        public string ProductSlug { get; set; }
        public double Price { get; set; }
    }
}