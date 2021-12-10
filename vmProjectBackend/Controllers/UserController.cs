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
        [HttpGet("admin/allusers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            string user_email = HttpContext.User.Identity.Name;
            var auth_user = _context.Users
                            .Where(p => p.email == user_email && p.userType == "Professor")
                            .FirstOrDefault();

            if (auth_user != null)
            {
                return await _context.Users.ToListAsync();
            }
            return Unauthorized();

        }
        // get all user who are professors
        [HttpGet("admin/allprofessors")]
        public async Task<ActionResult<IEnumerable<User>>> GetProfessors()
        {
            string user_email = HttpContext.User.Identity.Name;
            var auth_user = _context.Users
                            .Where(p => p.email == user_email && p.userType == "Professor")
                            .FirstOrDefault();

            if (auth_user != null)
            {
                return await _context.Users.Where(u => u.userType == "Professor").ToListAsync();
            }
            return Unauthorized();

        }

        // needs to be fixed to get any user in the database
        // GET: api/User/5
        [HttpGet("admin/specificUser/{user_id}")]
        public async Task<ActionResult<User>> GetUser(Guid user_id)
        {
            string user_email = HttpContext.User.Identity.Name;
            var auth_user = _context.Users
                            .Where(p => p.email == user_email && p.userType == "Professor")
                            .FirstOrDefault();

            if (auth_user != null)
            {
                var specific_user = await _context.Users.Where(u => u.UserID == user_id).FirstOrDefaultAsync();
                return Ok(specific_user);
            }
            return Unauthorized();


        }

        // add/register a professor to a database
        [HttpPost("admin/createuser")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var auth_user = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (auth_user != null)
            {
                var validemail = _context.Users
                            .Where(p => p.email == user.email)
                            .FirstOrDefault();
                if (validemail == null)
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    return Ok("created");
                }
                else
                {
                    return Conflict("user with same Email already exits");
                }

            }
            return Unauthorized();
        }

        // Update/patch a user
        [HttpPatch("admin/updateuser/{id}")]
        public async Task<ActionResult> PartialUpdate(Guid id, JsonPatchDocument<User> patchDoc)
        {
            string user_email = HttpContext.User.Identity.Name;
            var auth_user = _context.Users
                            .Where(p => p.email == user_email && p.userType == "Professor")
                            .FirstOrDefault();
            if (auth_user != null)
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
            return Unauthorized();
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // update the currect user
        [HttpPut]
        public async Task<IActionResult> PutUser(User user)
        {
            string user_email = HttpContext.User.Identity.Name;
            var auth_user = _context.Users
                            .Where(p => p.email == user_email && p.userType == "Professor")
                            .FirstOrDefault();

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


    }
}
