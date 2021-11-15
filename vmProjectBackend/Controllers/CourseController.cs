using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly VmContext _context;

        public CourseController(VmContext context)
        {
            _context = context;
        }

        /****************************************
        Teachers should see a list of their classes 
        that they have for that semester and that section

        **************************************/

        // GET: api/Course/1/winter
        [HttpGet("{sectionnum}/{course_semester}")]
        public async Task<ActionResult> Get_Courses_section_specific(string sectionnum, string course_semester)
        {
            // grabbing the user that signed in
            string useremail = HttpContext.User.Identity.Name;

            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefault();

            // Console.WriteLine("this is the user email" + useremail);
            if (user_prof != null)
            {
                var listOfCourse = await _context.Enrollments
                                .Include(c => c.Course)
                                .Where(u => u.UserId == user_prof.UserID
                                         && u.teacherId == user_prof.UserID
                                         && u.section_num == sectionnum
                                         && u.semester == course_semester)
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

        ****************************************/
        // GET: api/Course/fall
        [HttpGet("specificsemester/{course_semester}")]
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
                                .ToListAsync();
                return Ok(listOfCourse);
            }

            else
            {
                return NotFound("You are not Authorized and not a Professor");
            }
        }

        /***********************************
        Teacher searching for a specifc course

        ********************************/
        // GET: api/Course/5/3/fall
        [HttpGet("specificcourse/{id}/{section_num}/{semester}")]
        public async Task<ActionResult<Course>> GetCourse(long id, string section_num, string semester)
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
                                .Where(c => c.CourseID == id
                                        && c.section_num == section_num
                                        && c.semester == semester
                                        && c.UserId == user_prof.UserID)
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
        [HttpGet("studentsincourse/{sectionnum}/{course_semester}/{course_Id}")]
        public async Task<ActionResult> Get_Students_section_specific(string sectionnum, string course_semester, long course_Id)
        {
            // grabbing the user that signed in
            string useremail = HttpContext.User.Identity.Name;

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
                                        && u.section_num == sectionnum
                                        && u.semester == course_semester
                                        && u.Course.CourseID == course_Id
                                        && u.User.userType == "Student"
                                        )
                                .ToListAsync();
                return Ok(listOfCourse);
            }

            else
            {
                return NotFound("You are not Authorized and not a Professor");
            }
        }


        /***********************************************
        Teachers change their course
        ************************/

        // PUT: api/Course/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(long id, Course course)
        {
            if (id != course.CourseID)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /**************************
        
        Teacher change course
        ****************/

        // POST: api/Course
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                _context.Courses.Add(course);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (CourseExists(course.CourseID))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetCourse", new { id = course.CourseID }, course);
            }
            return Unauthorized("You are not a professor");

        }


        /**************************************************
        Teacher delete course
        
        ********************/

        // DELETE: api/Course/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(long id)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                var singleCourse = _context.Enrollments
                                    .Include(c => c.Course)
                                    .Where(c => c.CourseID == id && c.UserId == user_prof.UserID)
                                    .FirstOrDefault();
                var course = await _context.Courses.FindAsync(id);
                if (course == null)
                {
                    return NotFound();
                }

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            return Unauthorized("You are not a professor");


            /***************************************************/

        }

        private bool CourseExists(long id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }


        /************************************************
        
        Teacher patch their course

        ***********************/


        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdate(long id, JsonPatchDocument<Course> patchDoc)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();


            // not complete as yet
            if (user_prof != null)
            {
                var orginalCourse = await _context.Courses.FirstOrDefaultAsync(c => c.CourseID == id);

                if (orginalCourse == null)
                {
                    return NotFound();
                }

                patchDoc.ApplyTo(orginalCourse, ModelState);
                await _context.SaveChangesAsync();

                return Ok(orginalCourse);


            }
            return Unauthorized("You are not Authorized and is not a Professor");

        }
    }
}
