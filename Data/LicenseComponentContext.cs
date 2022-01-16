using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftwareFullComponents.LicenseComponent.Models;

namespace SoftwareFullComponents.LicenseComponent.Data
{
    public class LicenseComponentContext : DbContext
    {
        public LicenseComponentContext (DbContextOptions<LicenseComponentContext> options)
            : base(options)
        {
            
        }
        
        public DbSet<License> License { get; set; }
    }
}
