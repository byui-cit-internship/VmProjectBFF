using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using Microsoft.AspNetCore.Authorization;
using vmProjectBackend.Services;

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : ControllerBase
    {
        private readonly DatabaseContext _context; 
        private readonly Authorization _auth;

        public StudentCourseController(DatabaseContext context)
        {
            _context = context;
            _auth = new Authorization(_context);
        }

        //Student get to see all their classes that they are enrolled in
        // GET: api/StudentCourse
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a Student
            User student = _auth.getUser(useremail);
            if (student != null)
            {
<<<<<<< HEAD
                // give the enrollment, user and vmtable data  
                var listOfCourse = await _context.Enrollments
                                    .Include(c => c.Course)
                                    .Where(s => s.UserId == user_student.UserID)
                                    .Select(e => new
                                    {
                                        student_name = $"{e.User.firstName} {e.User.lastName}",
                                        course_name = e.Course.CourseName,
                                        course_id = e.CourseID,
                                        template_id=e.Course.TemplateVm,
                                        course_semester = e.semester,
                                        enrollment_id = e.EnrollmentID
                                    })
                                    .ToArrayAsync();

                return Ok(listOfCourse);
            }
            return Unauthorized("You are not an Authorized User");
        }

        // Student get to see a specific class that they are enrolled in for a specific semester
        [HttpGet("student/course/{course_id}/{course_semester}/{sectionNum}")]
        public async Task<ActionResult<Course>> GetSpecificCourse(long course_id, string course_semester, string sectionNum)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a Student
            var user_student = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Student")
                            .FirstOrDefault();

            if (user_student != null)
            {
                var specificcourse = await _context.Enrollments
                                    .Include(c => c.Course)
                                    .Where(sp => sp.UserId == user_student.UserID
                                                && sp.CourseID == course_id
                                                && sp.semester == course_semester
                                            )

                                    .Select(e => new
                                    {
                                        student_name = $"{e.User.firstName} {e.User.lastName}",
                                        course_name = e.Course.CourseName,
                                        course_id = e.CourseID,
                                        course_semester = e.semester,
                                        course_vm = e.VmTableID
                                        
                                      
                                        
                                    })
                                    .FirstOrDefaultAsync();

                return Ok(specificcourse);
            }
            return Unauthorized("You are not Authorized User");
        }

=======
                var courseList = await (from u in _context.Users
                                                        join usr in _context.UserSectionRoles
                                                        on u.UserId equals usr.UserId
                                                        join s in _context.Sections
                                                        on usr.SectionId equals s.SectionId
                                                        join c in _context.Courses
                                                        on s.CourseId equals c.CourseId
                                                        where u.UserId == student.UserId
                                                        select new
                                                        {
                                                            course_id = s.SectionCanvasId,
                                                            course_name = c.CourseCode,
                                                            enrollment_id = usr.UserSectionRoleId,
                                                            student_name = $"{u.FirstName} {u.LastName}",
                                                            template_id = c.TemplateVm
                                                        }).ToArrayAsync();
                

                return Ok(courseList);
            }
            return Unauthorized("You are not an Authorized User");
        }
>>>>>>> auth-ebe
    }
}