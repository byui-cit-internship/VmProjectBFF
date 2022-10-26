using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vmProjectBFF.Exceptions;
using vmProjectBFF.Models;
using vmProjectBFF.Services;
using Newtonsoft.Json;

namespace vmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : BffController
    {
        public SemesterController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SemesterController> logger,
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

        /****************************************
         * Return A list of all semesters
         ***************************************/

        [HttpGet("semester")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetSemester()
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    dynamic thing = _backend.GetAllSemesters(professor.UserId);
                    return Ok(thing);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
        /****************************************
        Create Semesters
        ****************************************/
        [HttpPost("createSemester")]
        public async Task<ActionResult<Semester>> PostSemester([FromBody] Semester semester)
        {
            try
            {
                User admin = _authorization.GetAuth("admin");

                if (admin is not null && semester is not null)
                {
                    _lastResponse = _backendHttpClient.Get($"api/v2/Semester", new() { { "semesterId", semester.SemesterId } });
                    Semester semesterExist = JsonConvert.DeserializeObject<Semester>(_lastResponse.Response);
                    
                    if (semesterExist is null) {
                        semesterExist = new();
                        {
                            semesterExist.SemesterYear = semester.SemesterYear;
                            semesterExist.SemesterTerm = semester.SemesterTerm;
                            semesterExist.StartDate = semester.StartDate;
                            semesterExist.EndDate = semester.EndDate;
                        }
                        _lastResponse = _backendHttpClient.Post($"api/v2/Semester", semesterExist);
                        semesterExist = JsonConvert.DeserializeObject<Semester>(_lastResponse.Response);

                        return Ok("Semester ID " + semesterExist.SemesterId + ", " + semesterExist.SemesterTerm + ", " + semesterExist.SemesterYear + " was created");
                    }
                    else
                    {
                        return Conflict(new { message = $"A semester already exits with this id {semester.SemesterId}" });
                    }
                }
                return Unauthorized();
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}