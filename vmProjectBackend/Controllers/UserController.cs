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

namespace vmProjectBackend.Controllers
{
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.UserID)
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
                if (!UserExists(id))
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
        public async Task<IActionResult> DeleteUser(long id)
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

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }


        // Sending the email to the teacher based on student

        [HttpGet("sendemail/{studentid}")]

        public async Task<ActionResult<User>> GetEmail(long studentid)
        {
            /*gRAB THE PRAMS AND USE IT TO SEARCH THE DATA BASE FOR THAT USE 
            AND THEN SEND TO THE PROFESSOR THAT EMAIL*/

            // var user = await _context.Users.FindAsync(studentid);

            // if (user == null)
            // {
            //     return NotFound();
            // }
            // else
            // {
            //     Console.WriteLine(user.firstName);
            //     return Ok("you have found it");
            // }



            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("vmproject", "vmproject234@gmail.com"));
            mailMessage.To.Add(MailboxAddress.Parse("vmproject234@gmail.com"));
            mailMessage.Subject = "Test";
            mailMessage.Body = new TextPart("plain")
            {
                Text = "Hello from backend"
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
        [HttpGet("userdetails/{id}")]
        public async Task<IActionResult> UserDetails(int id)
        {
            // if (id == null)
            // {
            //     return NotFound();
            // }

            // searching for the a user that is enrolled in what course
            var userDetail = await _context.Users
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.UserID == id);

            // if the user is not found
            if (userDetail == null)
            {
                return NotFound();
            }

            return Ok(userDetail);
        }

    }
}
