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
                                                            course_name = c.CourseName,
                                                            enrollment_id = usr.UserSectionRoleId,
                                                            student_name = $"{u.FirstName} {u.LastName}",
                                                            template_id = c.TemplateVm
                                                        }).ToArrayAsync();
                

                return Ok(courseList);
            }
            return Unauthorized("You are not an Authorized User");
        }
    }
}