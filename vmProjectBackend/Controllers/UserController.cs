using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using Microsoft.AspNetCore.Authorization;
using vmProjectBackend.Services;

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly Authorization _auth;

        public UserController(DatabaseContext context)
        {
            _context = context;
            _auth = new Authorization(_context);
        }

        /****************************************
        Create or update a user in the database to have admin privileges
        ****************************************/
        [HttpPost("admin/createuser")]
        public async Task<ActionResult<User>> PostAdminUser(PostAdmin postUser)
        {
            // Gets email from session
            string userEmail = HttpContext.User.Identity.Name;

            // Returns a admin user or null if email is not associated with an administrator
            User admin = _auth.getAdmin(userEmail);

            if (admin != null)
            {
                // Get user object on the email provided by post
                User user = _auth.getUser(postUser.email);

                // If user doesn't exist, creae them with admin privileges
                if (user == null)
                {
                    user = new User();
                    user.Email = postUser.email;
                    user.FirstName = postUser.firstName;
                    user.LastName = postUser.lastName;
                    user.IsAdmin = true;
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    return Ok("created user as admin");
                }
                // Else edit found user to be an admin
                else
                {
                    user.IsAdmin = true;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return Ok("modified user to be admin");
                }
            }
            return Unauthorized();
        }
    }
}
