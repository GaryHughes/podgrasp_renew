using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Podgrasp.Service.Model;

namespace Podgrasp.Service.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]/{version:apiVersion}/[action]")]
    public class ApiController : ControllerBase
    {
        readonly ILogger<ApiController> _logger;

        readonly PodgraspContext _context;

        public ApiController(ILogger<ApiController> logger, PodgraspContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Podcasts()
        {
            return new JsonResult(_context.Podcasts);                    
        }

    }
}