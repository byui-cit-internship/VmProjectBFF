using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using vmProjectBackend.Services;

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly Authorization _auth;

        ILogger Logger { get; } = AppLogger.CreateLogger<CourseController>();
        public IHttpClientFactory _httpClientFactory { get; }

        public CourseController(DatabaseContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _auth = new Authorization(_context);
        }

        /****************************************
        Returns secions taught by a professor in a given semester
        ****************************************/
        [HttpGet("professor/semester/{course_semester}")]
        public async Task<ActionResult> GetCoursesBySemester(string semester)
        {
            // Gets email from session
            string userEmail = HttpContext.User.Identity.Name;

            // Returns a professor user or null if email is not associated with a professor
            User professor = _auth.getAdmin(userEmail);

            if (professor != null)
            {
                // Returns a list of course name, section id, semester, section number, and professor
                // based on the professor and semester variables
                var listOfCourse = (from c in _context.Courses
                                    join sec in _context.Sections
                                    on c.CourseId equals sec.CourseId
                                    join sem in _context.Semesters
                                    on sec.SemesterId equals sem.SemesterId
                                    join usr in _context.UserSectionRoles
                                    on sec.SectionId equals usr.SectionId
                                    join u in _context.Users
                                    on usr.UserId equals u.UserId
                                    where u.Email == professor.Email
                                    && sem.SemesterTerm == semester
                                    select new
                                    {
                                        course_name = c.CourseName,
                                        course_id = sec.SectionId,
                                        course_semester = sem.SemesterTerm,
                                        course_section = sec.SectionNumber,
                                        course_professor = $"{u.FirstName} {u.LastName}"
                                    }).ToList();

                return Ok(listOfCourse);
            }

            else
            {
                return NotFound("You are not Authorized and not a Professor");
            }
        }

        /****************************************
        Checks canvas section id and canvas api key
        ****************************************/
        [HttpPost("professor/checkCanvasToken")]
        public async Task<ActionResult> CallCanvas([FromBody] CanvasCredentials canvasCredentials)
        {
            // Gets email from session
            string userEmail = HttpContext.User.Identity.Name;

            // Returns a professor user or null if email is not associated with a professor
            User professor = _auth.getAdmin(userEmail);

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
