using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;

namespace vmProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly VmContext _context;
        public TokenController(VmContext context)
        {
            _context = context;
        }

        /**************************************
        Validate the token given by the front end
        and then determines whether they are a teacher or professor
        ****************************************/
        [HttpGet("{token}")]
        public async Task<ActionResult<Token>> PostToken(string token)
        {
            try
            {
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(token);

                string validEmail = validPayload.Email;
                // Console.WriteLine(validEmail);

                var user_detail = _context.Users.Where(u => u.email == validEmail).FirstOrDefault();

                if (user_detail == null)
                {
                    try
                    {
                        string fullName = validPayload.Name;
                        string[] names = fullName.Split(' ');
                        string firstname = names[0];
                        string lastname = names[1];

                        User user = new User();
                        user.email = validPayload.Email;
                        user.firstName = firstname;
                        user.lastName = lastname;
                        user.userType = "Student";

                        _context.Users.Add(user); ;
                        await _context.SaveChangesAsync();
                        return Ok(user.userType);
                    }
                    catch (Exception ex)
                    {
                        return NotFound("hitting the error for trying to create a user");

                    }
                }

                // outside return statment
                return Ok(user_detail);
            }
            catch (Exception ex)
            {
                return NotFound("the token is not valid");

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToken(string id)
        {
            var token = await _context.Tokens.FindAsync(id);
            if (token == null)
            {
                return NotFound();
            }

            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool TokenExists(string id)
        {
            return _context.Tokens.Any(e => e.ID == id);
        }
    }
}
