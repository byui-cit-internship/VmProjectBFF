using System;
using System.Linq;
using System.Threading.Tasks;

// using System.Collections.IEm

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public EnrollmentController(DatabaseContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        /******************************************
        Teacher is able to Register themselves to a class.
        This will also create the class along with the enrollment 
        of themselves to that class. Along with a Vm Template assignment
        ***************************************/
        [HttpPost("professor/register/course")]
        public async Task<ActionResult<CourseCreate>> CreateCourseEnrollment([FromBody] CourseCreate courseDetails)
        {
            string useremail = HttpContext.User.Identity.Name;
            var user_prof = _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null && courseDetails != null)
            {
                // check if the course is already create
                var course_exit = _context.Courses.Where(p => p.CourseID == courseDetails.course_id).FirstOrDefault();
                if (course_exit == null)
                { // create the course
                    Course course = new Course();
                    course.CourseID = courseDetails.course_id;
                    course.CourseName = courseDetails.courseName;
                    course.description = courseDetails.description;
                    _context.Courses.Add(course);
                    await _context.SaveChangesAsync();

                    // Create the enrollment
                    Section enrollment = new Section();

                    var _courseObject = await _context.Courses
                                        .Where(c => c.CourseID == courseDetails.course_id)
                                        .FirstOrDefaultAsync();

                    enrollment.CourseID = _courseObject.CourseID;
                    enrollment.UserId = user_prof.UserID;
                    enrollment.teacherId = courseDetails.teacherId;
                    enrollment.VmTableID = courseDetails.vmTableID;
                    enrollment.Status = courseDetails.status;
                    enrollment.section_num = courseDetails.section_num;
                    enrollment.canvas_token = courseDetails.canvas_token;
                    enrollment.semester = courseDetails.semester;

                    _context.Enrollments.Add(enrollment);
                    await _context.SaveChangesAsync();

                    return Ok("ID " + enrollment.EnrollmentID + " enrollment was created");

                }
                else
                {
                    return Conflict(new { message = $"A course already exits with this id {courseDetails.course_id}" });
                }
            }
            return Unauthorized();
        }

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
                                                await EnrollStudent(course_id, current_student_enrollid, current_enrollment.teacherId, current_enrollment.VmTableID, current_enrollment.section_num, current_enrollment.semester);
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
                                            await EnrollStudent(course_id, student_user.UserID, current_enrollment.teacherId, current_enrollment.VmTableID, current_enrollment.section_num, current_enrollment.semester);

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

        public async Task EnrollStudent(long course_id, Guid userid, Guid teacherid, Guid vmtableId, string sectionnum, string semester)
        {
            Section enrollment = new Section();
            long enroll_course_id = _context.Courses.FirstOrDefault(c => c.CourseID == course_id).CourseID;
            enrollment.CourseID = enroll_course_id;
            enrollment.UserId = userid;
            enrollment.teacherId = teacherid;
            enrollment.VmTableID = vmtableId;
            enrollment.Status = "InActive";
            enrollment.section_num = sectionnum;
            enrollment.semester = semester;
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}
