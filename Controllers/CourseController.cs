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
using vmProjectBFF.Exceptions;
using vmProjectBFF.Models;
using vmProjectBFF.Services;

namespace vmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BffController
    {

        public CourseController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CourseController> logger)
            :base(
                  configuration: configuration,
                  httpClientFactory: httpClientFactory,
                  httpContextAccessor: httpContextAccessor,
                  logger: logger)
        {
        }

        /****************************************
         * 
         ***************************************/
        /**
         * <summary>
         * Returns a list of courses based on the semester.
         * </summary>
         * <returns>A list of "OldSectionDTO" objects repesenting information about sections the requesting professor teaches.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/course/professor/semester/{semester}
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      GET /api/course/professor/semester/fall
         *      RETURNS
         *      {
         *          [
         *              {
         *                  "courseName": "Into to Databases",
         *                  "sectionId": 23,
         *                  "semesterTerm": "Fall",
         *                  "sectionNumber": 1,
         *                  "fullName": "Michael Ebenal"
         *              },
         *              .
         *              .
         *              .
         *          ]
         *      }
         *
         * </remarks>
         * <response code="200">Returns a list of objects representing sections.</response>
         * <response code="403">Insufficent permission to make request.</response>
         */
        [HttpGet("professor/semester/{course_semester}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetCoursesBySemester(string semester)
        {
            try
            {
                User professor = _auth.getAuth("admin");

                if (professor != null)
                {
                    BffResponse sectionListResponse = _backend.Get($"api/v1/section/sectionList/{semester}");
                    List<OldSectionDTO> sectionList = JsonConvert.DeserializeObject<List<OldSectionDTO>>(sectionListResponse.Response);
                    return Ok(sectionList);
                }
                else
                {
                    return Forbid("You are not Authorized and not a Professor");
                }
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }


        /****************************************
        Checks canvas section id and canvas api key
        ****************************************/
        /**
         * <summary>
         * Checks whether a canvas token is valid.
         * </summary>
         * <returns>A copy of the information sent.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/course/professor/checkCanvasToken
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      POST /api/course/professor/checkCanvasToken
         *      BODY
         *      {
         *          "canvas_token": "sffgjlkdgjhsdsfdlkfjghsdfgsdlkjfgh",
         *          "canvas_course_id": 234125
         *      }
         *      RETURNS
         *      {
         *          "canvas_token": "sffgjlkdgjhsdsfdlkfjghsdfgsdlkjfgh",
         *          "canvas_course_id": 234125
         *      }
         *
         * </remarks>
         * <response code="200">Returns aa copy of the information that was sent.</response>
         * <response code="403">Insufficent permission to make request.</response>
         * <response code="413">Canvas token is incorrect/invalid</response>
         */
        [HttpPost("professor/checkCanvasToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(413)]
        public async Task<ActionResult> CallCanvas([FromBody] CanvasCredentials canvasCredentials)
        {
            // Returns a professor user or null if email is not associated with a professor
            User professor = _auth.getAuth("admin");

            if (professor != null)
            {
                var httpClient = HttpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, "Bearer " + canvasCredentials.canvas_token);
                // contains our base Url where individula course_id is added
                // This URL enpoint gives a list of all the Student in that class : role_id= 3 list all the student for that Professor
                var response = await httpClient.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{canvasCredentials.canvas_course_id}/enrollments?per_page=1000");

                if (response.IsSuccessStatusCode)
                {
                    return Ok(canvasCredentials);
                }
                return StatusCode(413, "Invalid token");
                // return Ok(canvasCredentials);
            }
            return Forbid("You are not Authorized and is not a Professor");

        }
    }
}
