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

// using System.Collections.IEm

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        public class CourseCreates
        {
            public string courseName { get; set; }
            public string description { get; set; }
            public string canvas_token { get; set; }
            public string section_num { get; set; }
            public string semester { get; set; }
            public long userId { get; set; }
            public long teacherId { get; set; }
            public long vmTableID { get; set; }
            public string status { get; set; }
        }


        private readonly VmContext _context;
        // public List<CourseCreate> coursedata = new List<CourseCreate>();

        public EnrollmentController(VmContext context)
        {
            _context = context;

        }
        [HttpPost("professor/register/course")]
        public async Task<ActionResult<CourseCreate>> CreateCourseEnrollment([FromBody] CourseCreates courseDetails)
        {
            string useremail = HttpContext.User.Identity.Name;
            var user_prof = _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null && courseDetails != null)
            {
                // create the course
                Course course = new Course();
                course.CourseName = courseDetails.courseName;
                course.description = courseDetails.description;
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                // Create the enrollment
                Enrollment enrollment = new Enrollment();

                var _courseObject = await _context.Courses
                                    .Where(c => c.CourseName == courseDetails.courseName)
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
            return Unauthorized();

        }
        /**********************************
        Teacher should be able to register a new class to a section
        along with a Vm for that class
        *********************************/
        // [HttpPost("professor/register/course")]
        // public async Task<ActionResult> CreateCourseEnrollment([FromBody] Object CourseDetails)
        // {
        //     string useremail = HttpContext.User.Identity.Name;
        //     var user_prof = _context.Users
        //                     .Where(p => p.email == useremail
        //                             && p.userType == "Professor")
        //                     .FirstOrDefault();

        //     if (user_prof != null)
        //     {
        //         if (CourseDetails != null)

        //         {
        //             // this deserialize the Json object to a .net object, It is then 
        //             // Cast to a Dictionary format which is then accessed by key value pair
        //             var list = JsonConvert.DeserializeObject<Dictionary<string, string>>(CourseDetails.ToString());

        //             // Create varibale for creating a new Course
        //             string coursename = list["courseName"];
        //             string coursedescription = list["description"];
        //             // Create a new course Object
        //             Course course = new Course();
        //             course.CourseName = coursename;
        //             course.description = coursedescription;
        //             // create the course
        //             _context.Courses.Add(course);
        //             await _context.SaveChangesAsync();
        //             // Create the enrollment
        //             Enrollment enrollment = new Enrollment();
        //             enrollment.UserId = user_prof.UserID;
        //             var courseid = await _context.Courses
        //                                 .Where(c => c.CourseName == coursename)
        //                                 .FirstOrDefaultAsync();

        //             enrollment.CourseID = courseid.CourseID;
        //             enrollment.Status = "Active";
        //             enrollment.section_num = list["section_num"];
        //             enrollment.semester = list["semester"];
        //             enrollment.teacherId = user_prof.UserID;
        //             // convert the string value of vamtable id to a long
        //             long vmtableid = long.Parse(list["vmTableID"]);
        //             enrollment.VmTableID = vmtableid;

        //             _context.Enrollments.Add(enrollment);
        //             await _context.SaveChangesAsync();

        //             return Ok("Enrollment was created");
        //         }
        //         else
        //         {
        //             return BadRequest("Enrollment Failed to Be Created");
        //         }
        //     }
        //     return Unauthorized();

        // }


        [HttpPost("professor/register/")]
        public async Task<ActionResult<Enrollment>> PostEnrollment([FromBody] Enrollment enrollment)
        {

            string useremail = HttpContext.User.Identity.Name;

            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefault();
            if (user_prof != null)
            {
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEnrollment", new { id = enrollment.EnrollmentID }, enrollment);
            }
            return Unauthorized("you are not authorized to create enrollment");
        }
    }
}














//         // GET: api/Enrollment
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments()
//         {
//             string useremail = HttpContext.User.Identity.Name;
//             var user_prof = _context.Users
//                             .Where(p => p.email == useremail && p.userType == "Professor")
//                             .FirstOrDefault();
//             if (user_prof != null)
//             {
//                 return await _context.Enrollments
//                             .Include(c => c.Course)
//                             .Include(u => u.User)
//                             .ToListAsync();
//             }
//             return Unauthorized("You are not Authorized");

//         }
//         /*********************Teacher getting theor course**************************************************/

//         //Get : api/enrollment/usertype
//         [HttpGet("usertype/{usertype}")]
//         public async Task<ActionResult<Enrollment>> GetUsertypeEnrollment(string usertype)
//         {
//             string user_email = HttpContext.User.Identity.Name;
//             Console.WriteLine("here 1");
//             var auth_user = _context.Users
//                             .Where(p => p.email == user_email)
//                             .FirstOrDefault();

//             Console.WriteLine("here 2");
//             if (auth_user != null)
//             {
//                 Console.WriteLine("here 3");
//                 var listOfEnrollments = await _context.Enrollments
//                             .Include(c => c.Course)
//                             .Include(u => u.User)
//                             .Where(user => user.User.userType == usertype)
//                             .ToListAsync();
//                 Console.WriteLine("here 1");
//                 return Ok(listOfEnrollments);

//             }
//             return Unauthorized();
//         }

//         /*********************Teacher getting theor course**************************************************/
//         // GET: api/Enrollment/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Enrollment>> GetEnrollment(long id)
//         {
//             string useremail = HttpContext.User.Identity.Name;
//             var user_prof = _context.Users
//                             .Where(p => p.email == useremail && p.userType == "Professor")
//                             .FirstOrDefault();
//             if (user_prof != null)
//             {
//                 var enrollment = await _context.Enrollments.FindAsync(id);
//                 if (enrollment == null)
//                 {
//                     return NotFound();
//                 }
//                 return enrollment;
//             }
//             return Unauthorized("You are not Authorized");
//         }

//         /*********************Teacher getting theor course**************************************************/

//         // PUT: api/Enrollment/5
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         // 
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutEnrollment(long id, Enrollment enrollment)
//         {
//             string useremail = HttpContext.User.Identity.Name;
//             var user_prof = _context.Users
//                             .Where(p => p.email == useremail && p.userType == "Professor")
//                             .FirstOrDefault();
//             if (user_prof != null)
//             {
//                 if (id != enrollment.EnrollmentID)
//                 {
//                     return BadRequest();
//                 }
//                 _context.Entry(enrollment).State = EntityState.Modified;
//                 try
//                 {
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!EnrollmentExists(id))
//                     {
//                         return NotFound();
//                     }
//                     else
//                     {
//                         throw;
//                     }
//                 }
//                 return NoContent();
//             }
//             return Unauthorized("You are not Authorized");
//         }


//         /*********************Teacher getting theor course**************************************************/
//         // POST: api/Enrollment
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPost]
//         public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
//         {
//             _context.Enrollments.Add(enrollment);
//             await _context.SaveChangesAsync();

//             return CreatedAtAction("GetEnrollment", new { id = enrollment.EnrollmentID }, enrollment);
//         }


//         /*********************Teacher getting theor course**************************************************/


//         // DELETE: api/Enrollment/5
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteEnrollment(long id)
//         {
//             string useremail = HttpContext.User.Identity.Name;
//             var user_prof = _context.Users
//                             .Where(p => p.email == useremail && p.userType == "Professor")
//                             .FirstOrDefault();

//             if (user_prof != null)
//             {
//                 var enrollment = await _context.Enrollments.FindAsync(id);
//                 if (enrollment == null)
//                 {
//                     return NotFound();
//                 }

//                 _context.Enrollments.Remove(enrollment);
//                 await _context.SaveChangesAsync();

//                 return NoContent();
//             }
//             return Unauthorized("You are not a professor");
//         }

//         private bool EnrollmentExists(long id)
//         {
//             return _context.Enrollments.Any(e => e.EnrollmentID == id);
//         }
//     }
// }
