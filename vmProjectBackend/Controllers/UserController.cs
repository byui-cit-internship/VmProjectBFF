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

        [HttpGet("sendemail/{id}")]

        public ActionResult<string> Get(int id)
        {
            // return Ok("you hit the endpoint");



            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("vmproject", "vmproject234@gmail.com"));
            mailMessage.To.Add(MailboxAddress.Parse("nol18003@byui.edu"));
            mailMessage.Subject = "Test";
            mailMessage.Body = new TextPart("plain")
            {
                Text = "Hello from backend"
            };

            SmtpClient client = new SmtpClient();

            try
            {
                Console.WriteLine("here 1");
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                Console.WriteLine("here 2");
                client.Authenticate("vmproject234@gmail.com", "vmProject199321");
                Console.WriteLine("here 3");
                client.Send(mailMessage);
                Console.WriteLine("here 4");

                return Ok("Message was sent");
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
            // using (var smtpClient = new SmtpClient())
            // {
            //     smtpClient.Connect("smtp.gmail.com", 587, true);
            //     smtpClient.Authenticate("vmproject234@gmail.com", "vmProject199321");
            //     smtpClient.Send(mailMessage);
            //     smtpClient.Disconnect(true);
            // }
            // return Ok("you have hit the email get end point");

            //     }
            //         catch (Exception)
            //         {
            //             return NotFound("did not send email");
            // }


        }


    }
}
