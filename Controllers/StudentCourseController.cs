using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vmProjectBFF.Models;
using Microsoft.AspNetCore.Authorization;
using vmProjectBFF.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using vmProjectBFF.DTO;
using Newtonsoft.Json;

namespace vmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : ControllerBase
    {
        private readonly Authorization _auth;
        private readonly Backend _backend;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<StudentCourseController> _logger;
        private BackendResponse _lastResponse;

        public StudentCourseController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<StudentCourseController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _auth = new(_backend, _logger);
            _httpClientFactory = httpClientFactory;
        }

        //Student get to see all their classes that they are enrolled in
        // GET: api/StudentCourse
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            try
            {
                User student = _auth.getAuth("user");
                if (student != null)
                {
                    _lastResponse = _backend.Get($"api/v1/StudentCourse", new { queryUserId = student.UserId });
                    List<CourseListByUserDTO> courseList = JsonConvert.DeserializeObject<List<CourseListByUserDTO>>(_lastResponse.Response);

                    return Ok(courseList);
                }
                return Unauthorized("You are not an Authorized User");
            }
            catch (BackendException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}