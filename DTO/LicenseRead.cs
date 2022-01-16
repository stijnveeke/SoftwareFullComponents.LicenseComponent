using System;

namespace SoftwareFullComponents.LicenseComponent.DTO
{
    public class LicenseRead
    {
        public int Id { get; set; }
        public Guid LicenseKey { get; set; }
        public ProductRead Product { get; set; }
        public UserRead User { get; set; }
        
        public int TimesActivated { get; set; }
        public int ActivateableAmount { get; set; }
        
        public string UserIdentifier { get; set; }
        public string ProductSlug { get; set; }
    }
}