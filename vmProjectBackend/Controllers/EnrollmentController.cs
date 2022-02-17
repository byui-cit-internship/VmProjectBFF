using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using vmProjectBackend.Services;

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
        private readonly Authorization _auth;

        public EnrollmentController(DatabaseContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _auth = new Authorization(_context);
        }

        /******************************************
        Teacher is able to Register themselves to a class.
        This will also create the class along with the enrollment 
        of themselves to that class. Along with a Vm Template assignment
        ***************************************/
        [HttpPost("professor/register/course")]
        public async Task<ActionResult<CourseCreate>> CreateCourseEnrollment([FromBody] CourseCreate courseDetails)
        {
            string userEmail = HttpContext.User.Identity.Name;

            User professor = _auth.getProfessor(userEmail);

            if (professor != null && courseDetails != null)
            {
                // check if the course is already create
                int courseExist = (from s in _context.Sections
                                   where s.SectionCanvasId == courseDetails.course_id
                                   select s.SectionCanvasId).Count();

                if (courseExist == 0)
                { // create the course
                    Course course = new Course();
                    course.CourseCode = courseDetails.courseName;
                    course.CourseName = courseDetails.courseName;
                    _context.Courses.Add(course);
                    _context.SaveChanges();

                    professor.CanvasToken = courseDetails.canvas_token;
                    _context.Users.Update(professor);
                    _context.SaveChanges();

                    Semester term = (from s in _context.Semesters
                                     where s.SemesterTerm == courseDetails.semester
                                     && s.SemesterYear == 2022
                                     select s).DefaultIfEmpty(null).FirstOrDefault();
                    
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

                    VmTemplate template = (from t in _context.VmTemplates
                                           where t.VmTemplateVcenterId == courseDetails.vmTableID
                                           select t).DefaultIfEmpty(null).FirstOrDefault();

                    if (template == null)
                    {
                        template = new VmTemplate();
                        template.VmTemplateVcenterId = courseDetails.vmTableID;
                        template.VmTemplateName = "test";
                        template.VmTemplateAccessDate = new DateTime(2022, 1, 1);
                    }

                    Section newSection = new Section();
                    newSection.Course = course;
                    newSection.SectionCanvasId = courseDetails.course_id;
                    newSection.Semester = term;
                    newSection.SectionNumber = courseDetails.section_num;
                    _context.Sections.Add(newSection);
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
