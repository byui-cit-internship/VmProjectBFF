using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;t
using vmProjectBackend.DTO;
using vmProjectBackend.Models;
using vmProjectBackend.Services;

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly Authorization _auth;
        private readonly Backend _backend;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CourseController> _logger;

        public IHttpClientFactory _httpClientFactory { get; }

        public CourseController(
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

        /****************************************
        Returns secions taught by a professor in a given semester
        ****************************************/
        [HttpGet("professor/semester/{course_semester}")]
        public async Task<ActionResult> GetCoursesBySemester(string semester)
        {
            try
            {


                // Returns a professor user or null if email is not associated with a professor
                User professor = _auth.getAuth("admin");

                if (professor != null)
                {
                    // Returns a list of course name, section id, semester, and professor
                    // based on the professor and semester variables
                    BackendResponse sectionListResponse = _backend.Get($"api/v1/section/sectionList/{semester}");
                    List<OldSectionDTO> sectionList = JsonConvert.DeserializeObject<List<OldSectionDTO>>(sectionListResponse.Response);
                    return Ok(sectionList);

                }

                else
                {
                    return NotFound("You are not Authorized and not a Professor");
                }
            }
            catch (BackendException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }

        /****************************************
        Checks canvas section id and canvas api key
        ****************************************/
        [HttpPost("professor/checkCanvasToken")]
        public async Task<ActionResult> CallCanvas([FromBody] CanvasCredentials canvasCredentials)
        {
            // Returns a professor user or null if email is not associated with a professor
            User professor = _auth.getAuth("admin");

            if (professor != null)
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, "Bearer " + canvasCredentials.canvas_token);
                // contains our base Url where individula course_id is added
                // This URL enpoint gives a list of all the Student in that class : role_id= 3 list all the student for that Professor
                var response = await httpClient.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{canvasCredentials.canvas_course_id}/enrollments?per_page=1000");

                if (response.IsSuccessStatusCode)
                {
                    return Ok(canvasCredentials);
                }
                return Unauthorized("Invalid token");
                // return Ok(canvasCredentials);
            }
            return Unauthorized("You are not Authorized and is not a Professor");

        }
    }
}
