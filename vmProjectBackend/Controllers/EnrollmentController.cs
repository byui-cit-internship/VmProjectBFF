using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
    public class EnrollmentController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Authorization _auth;
        private readonly BackgroundService1 _bs1;

        ILogger Logger { get; } = AppLogger.CreateLogger<EnrollmentController>();


        public EnrollmentController(DatabaseContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _auth = new Authorization(_context);
        }

        /****************************************
        Allows professor to create a course in the database using a canvas course id, a course name,
        a description, a canavs api token, a section number, a semester, and vm template id
        ****************************************/
        [HttpPost("professor/register/course")]
        public async Task<ActionResult<CourseCreate>> CreateCourseEnrollment([FromBody] CourseCreate courseDetails)
        {
            // Gets email from session
            string userEmail = HttpContext.User.Identity.Name;

            // Returns a professor user or null if email is not associated with a professor
            User professor = _auth.getAdmin(userEmail);

            if (professor != null && courseDetails != null)
            {
                // Check if a course already exists
                int courseExist = (from s in _context.Sections
                                   where s.SectionCanvasId == courseDetails.course_id
                                   select s.SectionCanvasId).Count();

                // If not, create course
                if (courseExist == 0)
                {
                    // Create new course
                    Course course = new Course();
                    course.CourseCode = courseDetails.courseName;
                    course.CourseName = courseDetails.courseName;
                    _context.Courses.Add(course);
                    _context.SaveChanges();

                    // Update professor's canvas api token
                    professor.CanvasToken = courseDetails.canvas_token;
                    _context.Users.Update(professor);
                    _context.SaveChanges();

                    // Return a semester from the database using provided semester term
                    Semester term = (from s in _context.Semesters
                                     where s.SemesterTerm == courseDetails.semester
                                     && s.SemesterYear == 2022
                                     select s).FirstOrDefault();

                    // If no semester exists, make a semester.
                    if (term == null)
                    {
                        term = new Semester();
                        term.SemesterTerm = courseDetails.semester;
                        term.SemesterYear = 2022;
                        term.StartDate = new DateTime(2022, 1, 1);
                        term.EndDate = new DateTime(2022, 12, 31);
                        _context.Semesters.Add(term);
                        _context.SaveChanges();
                    }

                    // Return a vm template from the database using the provided vsphere template id
                    VmTemplate template = (from t in _context.VmTemplates
                                           where t.VmTemplateVcenterId == courseDetails.vmTableID
                                           select t).FirstOrDefault();

                    // If template doesn't exist, create it
                    if (template == null)
                    {
                        template = new VmTemplate();
                        template.VmTemplateVcenterId = courseDetails.vmTableID;
                        template.VmTemplateName = "test";
                        template.VmTemplateAccessDate = new DateTime(2022, 1, 1);
                        _context.VmTemplates.Add(template);
                        _context.SaveChanges();
                    }

                    // Create section in database using provided canvas course id and section number,
                    // along with previous course and term
                    Section newSection = new Section();
                    newSection.Course = course;
                    newSection.SectionCanvasId = courseDetails.course_id;
                    newSection.Semester = term;
                    newSection.SectionNumber = courseDetails.section_num;
                    _context.Sections.Add(newSection);
                    _context.SaveChanges();

                    // Get role to signify that the person craeting this section is a professor
                    Role profRole = (from r in _context.Roles
                                     where r.RoleName == "Professor"
                                     select r).First();

                    // Create a link between the created section and the professor, effectively enrolling them
                    // in the class they created.
                    UserSectionRole enrollment = new UserSectionRole
                    {
                        UserId = professor.UserId,
                        RoleId = profRole.RoleId,
                        SectionId = newSection.SectionId
                    };
                    _context.UserSectionRoles.Add(enrollment);
                    _context.SaveChanges();

                    return Ok("ID " + newSection.SectionId + " enrollment was created");

                }
                else
                {
                    return Conflict(new { message = $"A course already exits with this id {courseDetails.course_id}" });
                }
            }
            return Unauthorized();
        }
    }
}
