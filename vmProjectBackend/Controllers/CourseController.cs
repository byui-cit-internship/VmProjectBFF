using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        /*******************************
        teacher should see a list of their classes in all section for a specific semester
        ****************************************/
        // GET: api/Course/fall
        [HttpGet("professor/semester/{course_semester}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult> GetCourses_semester(string course_semester)
        {
            // grabbing the user that signed in
            string useremail = HttpContext.User.Identity.Name;

            // check if it is a professor
            User professor = _auth.getProfessor(useremail);

            // Console.WriteLine("this is the user email" + useremail);
            if (professor != null)
            {
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
                                    && sem.SemesterTerm == course_semester
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
    }
}
