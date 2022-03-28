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
<<<<<<< HEAD
        Teachers should see a list of their classes 
        that they have for that semester and that section

        **************************************/

        // GET: api/Course/1/winter
        [HttpGet("professor/allcourses/{course_semester}/{sectionnum}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult> Get_Courses_section_specific(string course_semester, string sectionnum)
        {
            // grabbing the user that signed injj
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefault();

            // Console.WriteLine("this is the user email" + useremail);
            if (user_prof != null)
            {
                // Query the Enrollment table and doing a Join with Course table by using include
                var listOfCourse = await _context.Enrollments
                                .Include(c => c.Course)
                                .Where(u => u.UserId == user_prof.UserID
                                         && u.teacherId == user_prof.UserID
                                         && u.semester == course_semester)
                                         .Select(c => new
                                         {
                                             course_name = c.Course.CourseName,
                                             course_id = c.CourseID,
                                             course_semester = c.semester,
                                             course_professor = $"{c.User.firstName} {c.User.lastName}"
                                         })
                                .ToListAsync();
                return Ok(listOfCourse);
            }

            else
            {
                return NotFound("You are not Authorized and not a Professor");
            }
        }

        /*******************************
        teacher should see a list of their classes in all section for a specific semester

=======
        Returns secions taught by a professor in a given semester
>>>>>>> auth-ebe
        ****************************************/
        [HttpGet("professor/semester/{course_semester}")]
<<<<<<< HEAD
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult> GetCourses_semester(string course_semester)
        {
            // grabbing the user that signed in
            string useremail = HttpContext.User.Identity.Name;

            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            // Console.WriteLine("this is the user email" + useremail);
            if (user_prof != null)
            {
                var listOfCourse = await _context.Enrollments
                                .Include(c => c.Course)
                                .Where(u => u.UserId == user_prof.UserID
                                         && u.teacherId == user_prof.UserID
                                         && u.semester == course_semester)
                                .Select(c => new
                                {
                                    course_name = c.Course.CourseName,
                                    course_id = c.CourseID,
                                    course_semester = c.semester,
                                    course_professor = $"{c.User.firstName} {c.User.lastName}"
                                })
                                .ToListAsync();
                return Ok(listOfCourse);
            }

            else
            {
                return NotFound("You are not Authorized and not a Professor");
            }
        }

        /***********************************
        Teacher searching for a specifc course and check the VM for that class

        ********************************/
        // GET: api/Course/5/3/fall
        [HttpGet("professor/course/{course_Id}/{semester}/{sectionnum}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult<Course>> GetCourse(long course_Id, string semester, string sectionnum)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                var singleCourse = await _context.Enrollments
                                .Include(c => c.Course)
                                .Where(c => c.CourseID == course_Id
                                        && c.semester == semester
                                        && c.UserId == user_prof.UserID)
                                .Select(c => new
                                {
                                    course_name = c.Course.CourseName,
                                    course_id = c.CourseID,
                                    courses_semester = c.semester,
                                    course_vm = c.VmTableID
                                })
                                .ToListAsync();
                return Ok(singleCourse);
            }
            else
            {
                return NotFound("No such Course found");
            }
        }
        /********************************************
        Teacher wanting to know who is registered for their
        class
        **************************/
        [HttpGet("professor/students/{course_Id}/{course_semester}/{sectionnum}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult> Get_Students_section_specific(long course_Id, string course_semester, string sectionnum)
=======
        public async Task<ActionResult> GetCoursesBySemester(string semester)
>>>>>>> auth-ebe
        {
            // Gets email from session
            string userEmail = HttpContext.User.Identity.Name;

            // Returns a professor user or null if email is not associated with a professor
            User professor = _auth.getAdmin(userEmail);

            if (professor != null)
            {
                // Returns a list of course name, section id, semester, and professor
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
                                        course_professor = $"{u.FirstName} {u.LastName}"
                                    }).ToList();

<<<<<<< HEAD
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefault();

            // Console.WriteLine("this is the user email" + useremail);
            if (user_prof != null)
            {
                var listOfCourse = await _context.Enrollments
                                .Include(c => c.User)
                                .Include(c => c.Course)
                                .Where(u => u.teacherId == user_prof.UserID
                                        && u.semester == course_semester
                                        && u.Course.CourseID == course_Id
                                        && u.User.userType == "Student"
                                        )
                                .Select(s => new
                                {
                                    course_name = s.Course.CourseName,
                                    course_id = s.CourseID,
                                    course_semester = s.semester,
                                    student_name = $"{s.User.firstName} {s.User.lastName}",
                                    course_vm = s.VmTableID
                                })
                                .ToListAsync();
=======
>>>>>>> auth-ebe
                return Ok(listOfCourse);
            }

            else
            {
                return NotFound("You are not Authorized and not a Professor");
            }
        }
<<<<<<< HEAD
        /************************************************
        Teacher changing the status of a vm for a student
        ***********************/
        [HttpPatch("professor/changeVmStatus/{studentId}/{courseId}/{sectionNum}/{coursesemester}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult> ChangeVmStatus(Guid studentId, long courseId, string sectionNum, string coursesemester, JsonPatchDocument<Enrollment> patchDoc)
        {
            // verify it is a teacher
            string useremail = HttpContext.User.Identity.Name;
            var user_prof = _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefault();
            if (user_prof != null)
            {
                // need the StudentId and the student Enrollment, check if the enrollment
                //  has a teacher that matches with the teacher Id. Then do the patch else, Professor is not 
                // Authorized
                var student_enrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(c => c.UserId == studentId
                                       && c.teacherId == user_prof.UserID
                                       && c.CourseID == courseId
                                       && c.semester == coursesemester);

                if (student_enrollment == null)
                {
                    return NotFound();
                }
                patchDoc.ApplyTo(student_enrollment, ModelState);
                await _context.SaveChangesAsync();

                return Ok(student_enrollment);
            }
            return Unauthorized("You are not authorized");

        }

        /***********************************************
        Teachers change their course
        ************************/

        // PUT: api/Course/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{course_id}")]
        public async Task<IActionResult> PutCourse(long course_id, Course course)
        {
            if (course_id != course.CourseID)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;
=======
>>>>>>> auth-ebe

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
