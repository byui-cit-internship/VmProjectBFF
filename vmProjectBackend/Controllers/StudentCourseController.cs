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
                                        course_id = e.CourseID,
                                        course_semester = e.semester,
                                        course_sectionnum = e.section_num,
                                        course_status= e.Status, 
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
                                                && sp.section_num == sectionNum)

                                    .Select(e => new
                                    {
                                        student_name = $"{e.User.firstName} {e.User.lastName}",
                                        course_name = e.Course.CourseName,
                                        course_id = e.CourseID,
                                        course_semester = e.semester,
                                        course_sectionnum = e.section_num,
                                        course_vm_status = e.Status,
                                        course_vm = e.VmTableID
                                        
                                      
                                        
                                    })
                                    .FirstOrDefaultAsync();

                return Ok(specificcourse);
            }
            return Unauthorized("You are not Authorized User");
        }

        /***********************************************
        Student is able to send messeage to their Professor 
        to change their VM status fro that specific Class
        ************************************/
        [HttpGet("student/sendemail/{enrollmentID}")]

        public async Task<ActionResult<User>> GetEmail(Guid enrollmentID)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a Student
            var user_student = _context.Users.Where(p => p.email == useremail
                                                    && p.userType == "Student")
                                             .FirstOrDefault();
                                             
            // Check if it is valid enrollment and if the student is enrolled in that class
            var enrollment_details = _context.Enrollments.Where(p => p.EnrollmentID == enrollmentID
                                                                && p.UserId == user_student.UserID)
                                                         .FirstOrDefault();

            var professor_Id = enrollment_details.teacherId;
            // Double check that this is valid teacher and get their details
            var professor_details = _context.Users.Where(p => p.UserID == professor_Id
                                                        && p.userType == "Professor")
                                                  .FirstOrDefault();
            var professor_email = professor_details.email;

            // Getting the details of that course so that we can send the name of the course to the professor
            var courseid = enrollment_details.CourseID;
            var course_details = _context.Courses.Where(p => p.CourseID == courseid)
                                                 .FirstOrDefault();

            var className = course_details.CourseName;

            if (user_student != null && professor_details != null)
            {
                Console.WriteLine(className);
                string studentName = $"{user_student.firstName} {user_student.lastName}";
                Console.WriteLine(studentName);
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress(studentName, "vmproject234@gmail.com"));
                mailMessage.To.Add(MailboxAddress.Parse($"{professor_email}"));
                mailMessage.Subject = "Vm Request (Test)";
                mailMessage.Body = new TextPart("plain")
                {
                    Text = $"{studentName} needs permission to activate Vm for {className} in Section {enrollment_details.section_num}"
                };

                SmtpClient client = new SmtpClient();

                try
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("vmproject234@gmail.com", "vmProject199321");
                    client.Send(mailMessage);
                    return Ok("message was sent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return NotFound("not sucessfull");
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }

            }
            return Unauthorized();

        }
    }
}
