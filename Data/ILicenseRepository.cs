using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftwareFullComponents.LicenseComponent.DTO;
using SoftwareFullComponents.LicenseComponent.Models;

namespace SoftwareFullComponents.LicenseComponent.Data
{
    public interface ILicenseRepository
    {
        public Task<License> GetLicenseByLicenseKey(Guid licenseKey);
        public Task<IEnumerable<License>> GetLicenses();
        public Task<License> CreateLicense(License license);
    }
}