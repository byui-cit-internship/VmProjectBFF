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
    public class StudentCourseController : ControllerBase
    {
        private readonly VmContext _context;

        public StudentCourseController(VmContext context)
        {
            _context = context;
        }

        // GET: api/StudentCourse
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_student = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Student")
                            .FirstOrDefault();
            if (user_student != null)
            {
                var listOfCourse = _context.Enrollments
                                    .Include(c => c.Course)
                                    // .Include(vm => vm.VmTable)
                                    .Where(s => s.UserId == user_student.UserID);
                return Ok(listOfCourse);
            }
            return Unauthorized("You are not an Authorized User");


        }

        // // GET: api/StudentCourse/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Course>> GetCourse(long id)
        // {
        //     var course = await _context.Courses.FindAsync(id);

        //     if (course == null)
        //     {
        //         return NotFound();
        //     }

        //     return course;
        // }

        // // PUT: api/StudentCourse/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutCourse(long id, Course course)
        // {
        //     if (id != course.CourseID)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(course).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!CourseExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/StudentCourse
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Course>> PostCourse(Course course)
        // {
        //     _context.Courses.Add(course);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetCourse", new { id = course.CourseID }, course);
        // }

        // // DELETE: api/StudentCourse/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteCourse(long id)
        // {
        //     var course = await _context.Courses.FindAsync(id);
        //     if (course == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Courses.Remove(course);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        // private bool CourseExists(long id)
        // {
        //     return _context.Courses.Any(e => e.CourseID == id);
        // }
    }
}
