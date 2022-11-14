using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VmProjectBFF.DTO.Canvas;
using VmProjectBFF.DTO.Database;
using VmProjectBFF.Exceptions;
using VmProjectBFF.Services;

namespace VmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BffController
    {
        public CourseController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CourseController> logger,
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
        [HttpGet("professor/semester/{semester}")]


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetCoursesBySemester(string semester)
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    return Ok(_backend.GetSectionBySemester(semester));
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
         * 
         ***************************************/
        /**
         * <summary>
         * Returns a list of current courses by a professor for the semester.
         * </summary>
         * <returns>A list of "OldSectionDTO" objects repesenting information about sections the requesting professor teaches.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/course/professor/getAllCourses{semester}
         *          </code>
         *      </pre>
         
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      GET /api/course/professor/getAllCourses/fall
         *      RETURNS
         *      [
    {
        "courseId": 32,
        "courseCode": "CIT 171",
        "resourceGroupId": 1060
    },
    {
        "courseId": 33,
        "courseCode": "CIT 171",
        "resourceGroupId": 1060
    }
]
         *
         * </remarks>
         * <response code="200">Returns a list of objects representing sections.</response>
         * <response code="403">Insufficent permission to make request.</response>
         */
        [HttpGet("professor/getAllCourses")]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetAllCourses()
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    return Ok(_backend.GetCoursesByUserId(professor.UserId));
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
         * 
         ***************************************/
        /**
         * <summary>
         * Returns a list of sections been taught by a professor for the semester.
         * </summary>
         * <returns>A list of "OldSectionDTO" objects repesenting information about sections the requesting professor teaches.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/course/professor/getAllSections{semester}  
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      GET /api/course/professor/getAllSections
         *      RETURNS
         *      [

    {

        "sectionId": 19,

        "courseId": 32,

        "semesterId": 11,

        "folderId": 7,

        "resourceGroupId": 1074,

        "sectionNumber": 1,

        "sectionCanvasId": 195840,

        "sectionName": "2:00 PM Introduction to Cybersecurity",

        "libraryVCenterId": "db09653f-4963-4452-8abf-02656a9957b8"

    }
]
         *
         * </remarks>
         * <response code="200">Returns a list of objects representing sections.</response>
         * <response code="403">Insufficent permission to make request.</response>
         */
        [HttpGet("professor/getAllSections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetAllSections()
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    return Ok(_backend.GetSectionsByUserId(professor.UserId));
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
         * 
         ***************************************/
        /**
         * <summary>
         * Returns a dropdown of courses on Canvas for a professor.
         * </summary>
         * <returns>A list of "OldSectionDTO" objects repesenting information about sections the requesting professor teaches.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/course/professor/canvasDropdown
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      GET /api/course/professor/canvasDropdown
         *      RETURNS
         *     [
    {
        "id": 195840,
        "name": "2:00 PM Introduction to Cybersecurity",
        "account_id": 65,
        "uuid": "hoFhLxYiaw1WrAVxK1GLhRaSkImGNhHQZ93t5fjG",
        "start_at": "2022-09-13T06:00:00Z",
        "grading_standard_id": 1,
        "is_public": false,
        "created_at": "2022-06-21T16:08:20Z",
        "course_code": "CIT 171",
        "default_view": "wiki",
        "root_account_id": 1,
        "enrollment_term_id": 291,
        "license": "private",
        "grade_passback_setting": null,
        "end_at": null,
        "public_syllabus": false,
        "public_syllabus_to_auth": true,
        "storage_quota_mb": 2000,
        "is_public_to_auth_users": false,
        "homeroom_course": false,
        "course_color": null,
        "friendly_name": null,
        "apply_assignment_group_weights": false,
        "locale": "en",
        "calendar": {
            "ics": "https://byui.test.instructure.com/feeds/calendars/course_hoFhLxYiaw1WrAVxK1GLhRaSkImGNhHQZ93t5fjG.ics"
        },
        "time_zone": "America/Denver",
        "original_name": "Introduction to Cybersecurity",
        "blueprint": false,
        "template": false,
        "sis_course_id": "Campus.2022.Fall.CIT 171.3",
        "integration_id": null,
        "enrollments": [
            {
                "type": "teacher",
                "role": "TeacherEnrollment",
                "role_id": 4,
                "user_id": 250309,
                "enrollment_state": "active",
                "limit_privileges_to_course_section": true
            }
        ],
        "hide_final_grades": false,
        "workflow_state": "available",
        "restrict_enrollments_to_course_dates": false
    }
]
         *
         * </remarks>
         * <response code="200">Returns a list of objects representing sections.</response>
         * <response code="403">Insufficent permission to make request.</response>
         */
        [HttpGet("professor/canvasDropdown")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> canvasDropdown()
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    return Ok(_canvas.GetCoursesByCanvasToken(professor.CanvasToken));
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
            try
            {
                // Returns a professor user or null if email is not associated with a professor
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    // contains our base Url where individula course_id is added
                    // This URL enpoint gives a list of all the Student in that class : role_id= 3 list all the student for that Professor
                    _canvas.GetCoursesByCanvasToken(canvasCredentials.canvas_token);
                    return Ok(canvasCredentials);
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
    }
}
