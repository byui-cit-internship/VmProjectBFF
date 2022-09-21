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
        private readonly ILogger<TestController> _logger;

        public IHttpClientFactory _httpClientFactory { get; }

        public TestController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TestController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _auth = new(_backend, _logger);
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{var}")]
        public async Task<ActionResult> GetTest([FromRoute]string var)
        {
            dynamic data = $"{{\"data\": \"Your variable var is equal to {var}\"}}";
            return StatusCode(200, data);
        }

        [HttpGet()]
        public async Task<ActionResult> GetTestQuery([FromQuery] string username, [FromQuery] int? age)
        {
            dynamic data = $"{{\"data\": \"Your name is {username} and your age is {age}\"}}";
            return StatusCode(200, data);
        }

        [HttpPost()]
        public async Task<ActionResult> PostTest([FromBody]dynamic data)
        {
            dynamic data2 = $"{{\"data\": \"Your name is {data.name} and your age is {data.age}\"}}";
            return StatusCode(200, data2);
        }

        [HttpPut("{uniqueId}")]
        public async Task<ActionResult> PutTest([FromRoute] int uniqueId, [FromBody] dynamic data)
        {
            string name, newName;
            int age;
            int? newAge;
            switch (uniqueId) {
                case 1:
                    name = "michael";
                    age = 24;
                    break;
                case 2:
                    name = "murdock";
                    age = 35;
                    break;
                case 3:
                    name = "jaren";
                    age = 23;
                    break;
                default:
                    name = "unknown";
                    age = 0;
                    break;
            }

            newAge = data.age;
            newName = data.name;

            if (null != newAge)
            {
                age = (int)newAge;
            }
            if (null != newName)
            {
                name = newName;
            }

            dynamic data2 = $"{{\"data\": \"Your name is {name} and your age is {age}\"}}";
            return StatusCode(200, data2);
        }
    }
}
