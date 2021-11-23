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
    public class UserController : ControllerBase
    {
        private readonly VmContext _context;

        public UserController(VmContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet("allusers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            string user_email = HttpContext.User.Identity.Name;
            var auth_user = _context.Users
                            .Where(p => p.email == user_email)
                            .FirstOrDefault();
            if (auth_user != null)
            {
                return await _context.Users.ToListAsync();
            }
            return Unauthorized();

        }

        // GET: api/User/5
        [HttpGet("specificUser")]
        public async Task<ActionResult<User>> GetUser()
        {
            string user_email = HttpContext.User.Identity.Name;
            var auth_user = await _context.Users
                            .Where(p => p.email == user_email)
                            .FirstOrDefaultAsync();
            if (auth_user != null)
            {
                return Ok(auth_user);
            }
            return Unauthorized();


        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // update the currect user
        [HttpPut("updateUser")]
        public async Task<IActionResult> PutUser(User user)
        {
            string user_email = HttpContext.User.Identity.Name;
            var auth_user = await _context.Users
                            .Where(p => p.email == user_email)
                            .FirstOrDefaultAsync();

            if (auth_user.UserID != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(auth_user.UserID))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }


        // Sending the email to the teacher based on student


        // [HttpGet("userdetails/{id}")]
        // public async Task<IActionResult> UserDetails(int id)
        // {
        //     // searching for the a user that is enrolled in what course
        //     var userDetail = await _context.Users
        //     .Include(s => s.Enrollments)
        //     .ThenInclude(e => e.Course)
        //     .AsNoTracking()
        //     .FirstOrDefaultAsync(m => m.UserID == id);

        //     // if the user is not found
        //     if (userDetail == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(userDetail);
        // }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdate(Guid id, JsonPatchDocument<User> patchDoc)
        {
            var orginalUser = await _context.Users.FirstOrDefaultAsync(c => c.UserID == id);

            if (orginalUser == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(orginalUser, ModelState);
            await _context.SaveChangesAsync();

            return Ok(orginalUser);
        }
    }
}
