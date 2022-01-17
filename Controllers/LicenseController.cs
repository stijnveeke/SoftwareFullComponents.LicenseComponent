using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftwareFullComponents.LicenseComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : Controller
    {
        private readonly ILogger<LicenseController> _logger;
        private readonly LicenseLogicInterface _licenseLogic;

        public LicenseController(ILogger<LicenseController> logger, LicenseLogicInterface licenseLogic)
        {
            _logger = logger;
            _licenseLogic = licenseLogic;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<LicenseRead>>> GetLicenses()
        {
            return Ok(await this._licenseLogic.GetLicenses());
        }
        
        [HttpGet]
        [Route("{licenseKey}")]
        public async Task<ActionResult<IEnumerable<LicenseRead>>> GetLicense(Guid licenseKey)
        {
            return Ok(await this._licenseLogic.GetLicense(licenseKey));
        }

        [HttpGet]
        [Route("{productId}/{licenseKey}/verify")]
        public async Task<ActionResult<string>> ValidateLicenseKey(Guid productId, Guid licenseKey)
        {
            return Ok(await _licenseLogic.CheckLicense(productId, licenseKey));
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> CreateLicense([FromBody] LicenseCreate licenseCreate)
        {
            LicenseRead license = await _licenseLogic.GenerateLicense(licenseCreate);
            
            return CreatedAtAction("GetLicense", new { licenseKey = license.LicenseKey}, license);
        }
    }
}