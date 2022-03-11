using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : ControllerBase
    {
        private readonly VmContext _context;
        public StudentCourseController(VmContext context)
        {
            _context = context;
        }

        //Student get to see all their classes that they are enrolled in
        // GET: api/StudentCourse
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a Student
            var user_student = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Student")
                            .FirstOrDefault();
            if (user_student != null)
            {
                // give the enrollment, user and vmtable data  
                var listOfCourse = await _context.Enrollments
                                    .Include(c => c.Course)
                                    .Where(s => s.UserId == user_student.UserID)
                                    .Select(e => new
                                    {
                                        student_name = $"{e.User.firstName} {e.User.lastName}",
                                        course_name = e.Course.CourseName,
                                        template_id = c.Course.TemplateVm,                                        
                                        course_id = e.CourseID,
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
                                        template_id = c.Course.TemplateVm,                                          
                                        course_semester = e.semester,
                                        course_vm = e.VmTableID
                                        
                                      
                                        
                                    })
                                    .FirstOrDefaultAsync();

                return Ok(specificcourse);
            }
            return Unauthorized("You are not Authorized User");
        }

    }
}
