using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftwareFullComponents.LicenseComponent.DTO;

namespace SoftwareFullComponents.LicenseComponent.Controllers
{
    [ApiController]
    [Route("[controller]/ws")]
    public class WebSocketController : ControllerBase
    {

        private readonly ILogger<WebSocketController> _logger;
        private readonly LicenseLogicInterface _licenseLogic;

        public WebSocketController(ILogger<WebSocketController> logger, LicenseLogicInterface logic)
        {
            _logger = logger;
            _licenseLogic = logic;
        }

        [HttpGet("License")]
        public async Task<IEnumerable<LicenseRead>> GetLicenses()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                return await _licenseLogic.GetLicenses();
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }
        }
        
        [HttpGet("License/{licenseKey}")]
        public async Task<LicenseRead> GetLicense(Guid licenseKey)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                return await _licenseLogic.GetLicense(licenseKey);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }
        }
    }
}