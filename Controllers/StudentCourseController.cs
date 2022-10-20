using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;
using vmProjectBFF.Models;
using vmProjectBFF.Services;

namespace vmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : BffController
    {

        public StudentCourseController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<StudentCourseController> logger,
            IVCenterRepository vCenter)
            : base(
                  authorization: authorization,
                  backend: backend,
                  canvas: canvas,
                  configuration: configuration,
                  httpClientFactory: httpClientFactory,
                  httpContextAccessor: httpContextAccessor,
                  logger: logger,
                  vCenter: vCenter)
        {
        }

        //Student get to see all their classes that they are enrolled in
        // GET: api/StudentCourse
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            try
            {
                User student = _authorization.GetAuth("user");
                if (student != null)
                {
                    _lastResponse = _backendHttpClient.Get($"api/v1/StudentCourse", new() { { "queryUserId", student.UserId } });
                    List<CourseListByUserDTO> courseList = JsonConvert.DeserializeObject<List<CourseListByUserDTO>>(_lastResponse.Response);

                    return Ok(courseList);
                }
                return Unauthorized("You are not an Authorized User");
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}