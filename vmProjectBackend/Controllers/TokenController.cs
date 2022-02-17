using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;

namespace vmProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public TokenController(DatabaseContext context)
        {
            _context = context;
        }

        /**************************************
        Validate the token given by the front end
        and then determines whether they are a teacher or professor
        ****************************************/
        [HttpGet("{token}")]
        public async Task<ActionResult> PostToken(string token)
        {
            try
            {
                GoogleJsonWebSignature.Payload validPayload = await GoogleJsonWebSignature.ValidateAsync(token);

                string validEmail = validPayload.Email;
                // Console.WriteLine(validEmail);

                User user = (from u in _context.Users
                             where u.Email == validEmail
                             select u).FirstOrDefault();

                if (user == null)
                {

                    string fullName = validPayload.Name;
                    string[] names = fullName.Split(' ');

                    user = new User();
                    user.Email = validPayload.Email;
                    user.FirstName = names.First();
                    user.LastName = names.Last();
                    user.IsAdmin = false;

                    _context.Users.Add(user); ;
                    _context.SaveChanges();

                }

                // outside return statment
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound($"the token is not valid {ex}");

            }
        }
    }
}
