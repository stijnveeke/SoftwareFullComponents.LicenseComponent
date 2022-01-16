using AutoMapper;
using SoftwareFullComponents.LicenseComponent.DTO;
using SoftwareFullComponents.LicenseComponent.Models;

namespace SoftwareFullComponents.LicenseComponent.Profiles
{
    public class LicenseProfile: Profile
    {
        public LicenseProfile()
        {
            CreateMap<License, LicenseRead>();
            CreateMap<LicenseRead, License>();
            CreateMap<LicenseCreate, License>();            
        }
    }
}