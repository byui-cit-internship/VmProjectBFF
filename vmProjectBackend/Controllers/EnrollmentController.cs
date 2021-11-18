using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;


namespace vmProjectBackend.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly VmContext _context;

        public EnrollmentController(VmContext context)
        {
            _context = context;
        }

        /**********************************
        Teacher should be able to enroll students in a specific class to a specific Vm
        *********************************/
        [HttpPost("professor/register/course")]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
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
