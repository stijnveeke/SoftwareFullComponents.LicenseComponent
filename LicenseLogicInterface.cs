using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftwareFullComponents.LicenseComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent
{
    public interface LicenseLogicInterface
    {
        public Task<LicenseRead> GenerateLicense(LicenseCreate licenseCreate);
        public Task<LicenseRead> GetLicense(Guid licenseKey);
        public Task<IEnumerable<LicenseRead>> GetLicenses();
    }
}