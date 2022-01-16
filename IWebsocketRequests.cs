using System.Collections.Generic;
using System.Threading.Tasks;
using SoftwareFullComponents.LicenseComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent
{
    public interface IWebsocketRequests
    {
        public Task<ProductRead> GetProductById(string productSlug);
        public Task<UserRead> GetUserByUserId(string user_id);

        public Task<IEnumerable<LicenseRead>> GetUsersForLicenseList(List<LicenseRead> licenses);
        public Task<IEnumerable<LicenseRead>> GetProductsForLicenseList(List<LicenseRead> licenses);
    }
}