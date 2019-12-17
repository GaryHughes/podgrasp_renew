using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Podgrasp.Service.Model;

namespace Podgrasp.Service.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]/{version:apiVersion}/[action]")]
    [Authorize]   
    public class ApiController : ControllerBase
    {
        readonly ILogger<ApiController> _logger;
        readonly PodcastService _service;

        public ApiController(ILogger<ApiController> logger,
                             PodcastService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult Podcasts()
        {
            try {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return new JsonResult(_service.Podcasts(userId));       
            }    
            catch (Exception ex) {
                return BadRequest(ex);
            }            
        }

        [HttpPost]
        public IActionResult Subscribe([FromBody] Subscription subscription)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _service.Subscribe(userId, subscription);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }

            return Accepted();    
        }

    }
}