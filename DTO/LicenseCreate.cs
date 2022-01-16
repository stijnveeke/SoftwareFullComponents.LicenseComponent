namespace SoftwareFullComponents.LicenseComponent.DTO
{
    public class LicenseCreate
    {
        public string ProductSlug { get; set; }
        public string UserIdentifier { get; set; }
        public int Amount { get; set; }
    }
}