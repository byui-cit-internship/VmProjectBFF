using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
<<<<<<< HEAD
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Net.Http.Headers;

// using System.Collections.IEm
=======
using vmProjectBackend.Services;
>>>>>>> auth-ebe

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
<<<<<<< HEAD
                    course.Section = courseDetails.section;
                    course.ContentLibrary = courseDetails.contentLibrary;
                    course.TemplateVm = courseDetails.templateVm;
                    course.Semester = courseDetails.semester;   
                    course.Description = courseDetails.description;
                    course.Folder = courseDetails.folder;

=======
                    course.ContentLibrary = courseDetails.contentLibrary;
                    course.TemplateVm = courseDetails.templateVm;
                    course.Semester = courseDetails.semester;
                    course.Description = courseDetails.description;
                    course.Folder = courseDetails.folder;
>>>>>>> auth-ebe
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

<<<<<<< HEAD
                    enrollment.CourseID = _courseObject.CourseID;
                    enrollment.UserId = user_prof.UserID;
                    enrollment.teacherId = courseDetails.teacherId;
                    enrollment.VmTableID = courseDetails.vmTableID;
               
                    enrollment.canvas_token = courseDetails.canvas_token;
                    enrollment.semester = courseDetails.semester;
=======
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
>>>>>>> auth-ebe

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
<<<<<<< HEAD

        // register students for the newly created course: this endpont needs to be tested
        [HttpPost("professor/register/student")]
        public async Task<ActionResult<CourseCreate>> CreateStudentEnrollment([FromBody] long course_id)
        {
            string useremail = HttpContext.User.Identity.Name;
            var user_prof = await _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefaultAsync();

            if (user_prof != null)
            {
                // checking that the professor has such a class enrollment
                var current_enrollment = await _context.Enrollments
                                           .Where(e => e.CourseID == course_id
                                                       && e.teacherId == user_prof.UserID)
                                           .FirstOrDefaultAsync();

                if (current_enrollment != null)
                {
                    var canvas_token = current_enrollment.canvas_token;

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Add(HeaderNames.Authorization, "Bearer " + canvas_token);

                    var response = await client.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{course_id}/enrollments?per_page=1000&role_id=3");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        dynamic listOfcurrent_studentObject = JsonConvert.DeserializeObject<dynamic>(responseString);

                        foreach (var student in listOfcurrent_studentObject)
                        {
                            if (student.Count != 0)
                            {
                                var student_id = student["user_id"];
                                var studentInfoResponse = await client.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{course_id}/users?search_term={student_id}");

                                if (studentInfoResponse.IsSuccessStatusCode)
                                {
                                    string studentResponseString = await studentInfoResponse.Content.ReadAsStringAsync();
                                    dynamic current_studentObject = JsonConvert.DeserializeObject<dynamic>(studentResponseString);
                                    if (current_studentObject.Count != 0)
                                    {
                                        var current_student_id = current_studentObject[0]["id"];
                                        string current_student_email = current_studentObject[0]["email"];
                                        string studentnames = current_studentObject[0]["name"];
                                        string[] names = studentnames.Split(' ');
                                        int lastIndex = names.GetUpperBound(0);
                                        string current_student_firstName = names[0];
                                        string current_student_lastName = names[lastIndex];

                                        var current_student_in_db = _context.Users.Where(u => u.email == current_student_email).FirstOrDefault();

                                        if (current_student_in_db != null)
                                        {
                                            var current_student_enrollment = _context.Enrollments.Where(e => e.UserId == current_student_in_db.UserID
                                                                                                && e.CourseID == course_id)
                                                                                                .FirstOrDefault();
                                            Guid current_student_enrollid = current_student_enrollment.UserId;
                                            if (current_student_enrollment == null)
                                            {
                                                await EnrollStudent(course_id, current_student_enrollid, current_enrollment.teacherId, current_enrollment.VmTableID, current_enrollment.semester);
                                                Console.WriteLine("Student enrolled into the course");
                                            }
                                        }
                                        else
                                        {
                                            User student_user = new User();

                                            student_user.firstName = current_student_firstName;
                                            student_user.lastName = current_student_lastName;
                                            student_user.email = current_student_email;
                                            student_user.userType = "Student";
                                            _context.Users.Add(student_user);
                                            await _context.SaveChangesAsync();
                                            Console.WriteLine("Student_user was created");
                                            await EnrollStudent(course_id, student_user.UserID, current_enrollment.teacherId, current_enrollment.VmTableID, current_enrollment.semester);

                                            // return Ok("student was created and enrolled");
                                        }
                                    }
                                }
                            }
                        }
                        return Ok("Students are being added to course");

                    }
                    return Unauthorized("was not allowed to call canvas Api");
                    // check if the student is already in the database

                }
                return NotFound("You do not have access to such course or not authorized");
            }
            return Unauthorized();
        }

        public async Task EnrollStudent(long course_id, Guid userid, Guid teacherid, Guid vmtableId, string semester)
        {
            Enrollment enrollment = new Enrollment();
            long enroll_course_id = _context.Courses.FirstOrDefault(c => c.CourseID == course_id).CourseID;
            enrollment.CourseID = enroll_course_id;
            enrollment.UserId = userid;
            enrollment.teacherId = teacherid;
            enrollment.VmTableID = vmtableId;
            enrollment.semester = semester;
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }
=======
>>>>>>> auth-ebe
    }
}
