using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using System.Web;
using System.Text;
using System.Text.Json;
using System.Collections;
using Newtonsoft.Json;
using System.Reflection;
using System.Net.Http;
using Microsoft.Net.Http.Headers;

// using System.Collections.IEm

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly VmContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        // public List<CourseCreate> coursedata = new List<CourseCreate>();

        public EnrollmentController(VmContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;

        }

        /******************************************
        Teacher is able to Register themselves to a class.
        This will also create the class along with the enrollment 
        of them selve to that class. Along with a Vm Template assignment
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
                    Enrollment enrollment = new Enrollment();

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
                        return Ok();
                    }
                    return Unauthorized("was not allowed to call canvas Api");
                    // check if the student is already in the database

                }
                return NotFound("You do not have access to such course or not authorized");
            }
            return Unauthorized();
        }
    }
}
