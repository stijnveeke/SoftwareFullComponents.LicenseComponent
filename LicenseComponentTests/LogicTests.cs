using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SoftwareFullComponents.LicenseComponent;
using SoftwareFullComponents.LicenseComponent.Data;
using SoftwareFullComponents.LicenseComponent.DTO;
using SoftwareFullComponents.LicenseComponent.Models;
using SoftwareFullComponents.LicenseComponent.Profiles;

namespace LicenseComponentTests
{
    [TestClass]
    public class LogicTests
    {
        private readonly Mock<IWebsocketRequests> _websocketRequests = new Mock<IWebsocketRequests>();
        private readonly Mock<ILicenseRepository> _licenseRepository = new Mock<ILicenseRepository>();
        private readonly LicenseLogic _licenseLogic;
        private readonly IMapper _mapper;
        
        public LogicTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile<LicenseProfile>();
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            _licenseLogic = new LicenseLogic(this._websocketRequests.Object, _licenseRepository.Object, this._mapper);
        }

        [TestMethod()]
        public async Task can_create_a_license()
        {
            _websocketRequests
                .Setup(websocketRequests => websocketRequests.GetUserByUserId("auth0|8967832880934879023"))
                .ReturnsAsync(new UserRead
                {
                    blocked = false,
                    email = "test@gmail.com",
                    email_verified = true,
                    name = "test@gmail.com",
                    user_id = "auth0|8967832880934879023"
                });

            _websocketRequests
                .Setup(websocketRequests => websocketRequests.GetProductById("super-awesome-software"))
                .ReturnsAsync(new ProductRead
                {
                    Id = 1,
                    Price = 10.00,
                    ProductName = "Super Awesome Software",
                    ProductSlug = "super-awesome-software"
                });

            _licenseRepository
                .Setup(licenseRepository => licenseRepository.CreateLicense(It.IsAny<License>()))
                .ReturnsAsync(new License
                {
                    UserIdentifier = "auth0|8967832880934879023",
                    Id = 1,
                    LicenseKey = Guid.NewGuid().ToString(),
                    TimesActivated = 0,
                    ActivateableAmount = 5
                });

            var licenseCreate = new LicenseCreate
            {
                UserIdentifier = "auth0|8967832880934879023",
                ProductSlug = "super-awesome-software",
                Amount = 5
            };

            var license = await _licenseLogic.GenerateLicense(licenseCreate);

            Assert.AreEqual(licenseCreate.UserIdentifier, license.User.user_id);
            Assert.AreEqual(licenseCreate.ProductSlug, license.Product.ProductSlug);
            Assert.AreEqual(licenseCreate.Amount, license.ActivateableAmount);
            
            _licenseRepository.Verify(licenseRepository => licenseRepository.CreateLicense(It.IsAny<License>()), Times.Once);
            _websocketRequests.Verify(websocketRequests => websocketRequests.GetUserByUserId("auth0|8967832880934879023"), Times.Once);
            _websocketRequests.Verify(websocketRequests => websocketRequests.GetProductById("super-awesome-software"), Times.Once);
        }
        
        [TestMethod()]
        public async Task can_not_create_a_license_when_user_is_null()
        {
            _websocketRequests
                .Setup(websocketRequests => websocketRequests.GetUserByUserId("auth0|8967832880934879023"))
                .ReturnsAsync((UserRead)null);

            _websocketRequests
                .Setup(websocketRequests => websocketRequests.GetProductById("super-awesome-software"))
                .ReturnsAsync(new ProductRead
                {
                    Id = 1,
                    Price = 10.00,
                    ProductName = "Super Awesome Software",
                    ProductSlug = "super-awesome-software"
                });

            _licenseRepository
                .Setup(licenseRepository => licenseRepository.CreateLicense(It.IsAny<License>()))
                .ReturnsAsync(new License
                {
                    UserIdentifier = "auth0|8967832880934879023",
                    Id = 1,
                    LicenseKey = Guid.NewGuid().ToString(),
                    TimesActivated = 0,
                    ActivateableAmount = 5
                });

            var licenseCreate = new LicenseCreate
            {
                UserIdentifier = "auth0|8967832880934879023",
                ProductSlug = "super-awesome-software",
                Amount = 5
            };

            var license = await _licenseLogic.GenerateLicense(licenseCreate);

            Assert.AreEqual(null, license);
            
            // When user is not present license won't be created!
            _licenseRepository.Verify(licenseRepository => licenseRepository.CreateLicense(It.IsAny<License>()), Times.Never);
            
            _websocketRequests.Verify(websocketRequests => websocketRequests.GetUserByUserId("auth0|8967832880934879023"), Times.Once);
            _websocketRequests.Verify(websocketRequests => websocketRequests.GetProductById("super-awesome-software"), Times.Once);
        }
        
        
        [TestMethod()]
        public async Task can_not_create_license_when_product_is_null()
        {
            _websocketRequests
                .Setup(websocketRequests => websocketRequests.GetUserByUserId("auth0|8967832880934879023"))
                .ReturnsAsync(new UserRead
                {
                    blocked = false,
                    email = "test@gmail.com",
                    email_verified = true,
                    name = "test@gmail.com",
                    user_id = "auth0|8967832880934879023"
                });

            _websocketRequests
                .Setup(websocketRequests => websocketRequests.GetProductById("super-awesome-software"))
                .ReturnsAsync((ProductRead)null);

            _licenseRepository
                .Setup(licenseRepository => licenseRepository.CreateLicense(It.IsAny<License>()))
                .ReturnsAsync(new License
                {
                    UserIdentifier = "auth0|8967832880934879023",
                    Id = 1,
                    LicenseKey = Guid.NewGuid().ToString(),
                    TimesActivated = 0,
                    ActivateableAmount = 5
                });

            var licenseCreate = new LicenseCreate
            {
                UserIdentifier = "auth0|8967832880934879023",
                ProductSlug = "super-awesome-software",
                Amount = 5
            };

            var license = await _licenseLogic.GenerateLicense(licenseCreate);

            Assert.AreEqual(null, license);
            
            _licenseRepository.Verify(licenseRepository => licenseRepository.CreateLicense(It.IsAny<License>()), Times.Never);
            _websocketRequests.Verify(websocketRequests => websocketRequests.GetUserByUserId("auth0|8967832880934879023"), Times.Once);
            _websocketRequests.Verify(websocketRequests => websocketRequests.GetProductById("super-awesome-software"), Times.Once);
        }
    }
}