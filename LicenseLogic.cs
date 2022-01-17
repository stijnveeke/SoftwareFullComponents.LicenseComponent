using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoftwareFullComponents.LicenseComponent.Data;
using SoftwareFullComponents.LicenseComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent
{
    public class LicenseLogic: LicenseLogicInterface
    {
        private readonly IWebsocketRequests _websocketRequests;
        private readonly ILicenseRepository _licenseRepository;
        private readonly IMapper _mapper;
        
        public LicenseLogic(IWebsocketRequests websocketRequests, ILicenseRepository licenseRepository, IMapper mapper)
        {
            _websocketRequests = websocketRequests;
            _licenseRepository = licenseRepository;
            _mapper = mapper;
        }
        
        public async Task<LicenseRead> GenerateLicense(LicenseCreate licenseCreate)
        {
            UserRead user = await _websocketRequests.GetUserByUserId(licenseCreate.UserIdentifier);
            ProductRead product = await _websocketRequests.GetProductById(licenseCreate.ProductSlug);

            if (user != null && product != null)
            {
                Models.License license = new Models.License
                {
                    UserIdentifier = licenseCreate.UserIdentifier,
                    LicenseKey = Guid.NewGuid().ToString(),
                    ActivateableAmount = licenseCreate.Amount,
                    ProductId = product.Guid
                };

                var createdLicense = _mapper.Map<LicenseRead>(await this._licenseRepository.CreateLicense(license));
                createdLicense.User = user;
                createdLicense.Product = product;

                return createdLicense;
            }

            return null;
        }

        public async Task<bool> CheckLicense(Guid productId, Guid licenseKey)
        {
            return await _licenseRepository.CheckLicense(productId,licenseKey);
        }

        public async Task<IEnumerable<LicenseRead>> GetLicenses()
        {
            IEnumerable<LicenseRead> licenses = _mapper.Map<IEnumerable<LicenseRead>>(await this._licenseRepository.GetLicenses());

            licenses = await this._websocketRequests.GetUsersForLicenseList(licenses.ToList());
            licenses = await this._websocketRequests.GetProductsForLicenseList(licenses.ToList());
            
            return licenses;
        }

        public async Task<LicenseRead> GetLicense(Guid licenseKey)
        {
            return _mapper.Map<LicenseRead>(await this._licenseRepository.GetLicenseByLicenseKey(licenseKey));
        }
    }
}
