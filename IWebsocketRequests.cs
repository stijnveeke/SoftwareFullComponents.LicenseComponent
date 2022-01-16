using System.Threading.Tasks;
using SoftwareFullComponents.LicenseComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent
{
    public interface IWebsocketRequests
    {
        public Task<ProductRead> GetProductById(string productSlug);
        public Task<UserRead> GetUserByUserId(string user_id);
    }
}