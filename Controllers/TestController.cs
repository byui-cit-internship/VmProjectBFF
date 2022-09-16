using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using vmProjectBFF.DTO;
using vmProjectBFF.Models;
using vmProjectBFF.Services;

namespace vmProjectBFF.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Authorization _auth;
        private readonly Backend _backend;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CourseController> _logger;

        public IHttpClientFactory _httpClientFactory { get; }

        public TestController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CourseController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _auth = new(_backend, _logger);
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet()]
        public async Task<ActionResult> GetTest()
        {
            dynamic data = "{\"data\": \"You successfully called an endpoint\"}";
            return Ok(data);
        }
    }
}
