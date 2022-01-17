using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftwareFullComponents.LicenseComponent.Models;

namespace SoftwareFullComponents.LicenseComponent.Data
{
    public class LicenseRepository: ILicenseRepository
    {
        private readonly LicenseComponentContext _context;
        
        public LicenseRepository(LicenseComponentContext context)
        {
            _context = context;
        }
        
        public async Task<License> GetLicenseByLicenseKey(Guid licenseKey)
        {
            return await _context.License.Where(l => l.LicenseKey == licenseKey.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<License>> GetLicenses()
        {
            return await _context.License.ToListAsync();
        }

        public async Task<License> CreateLicense(License license)
        {
            await _context.License.AddAsync(license);
            await _context.SaveChangesAsync();

            return license;
        }

        public async Task<bool> CheckLicense(Guid productId, Guid licenseKey)
        {
            return (await _context.License.Where(l => l.LicenseKey == licenseKey.ToString() && l.ProductId == productId).CountAsync()) != 0;
        }
    }
}