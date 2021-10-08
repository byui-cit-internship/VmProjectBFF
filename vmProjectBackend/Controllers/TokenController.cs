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

        // GET: api/Token
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Token>>> GetTokens()
        {
            return await _context.Tokens.ToListAsync();
        }

        // GET: api/Token/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Token>> GetToken(string id)
        {
            var token = await _context.Tokens.FindAsync(id);

            if (token == null)
            {
                return NotFound();
            }

            return token;
        }

        // PUT: api/Token/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToken(string id, Token token)
        {
            if (id != token.ID)
            {
                return BadRequest();
            }

            _context.Entry(token).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TokenExists(id))
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

       
        [HttpPost]
        public async Task<ActionResult<Token>> PostToken(Token token)
        {

            string Id_token = token.token;

            // GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(Id_token);
            // ValidateIdTokenAndGetUserInfoId(Id_token);

            try
            {
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(Id_token);

                string validEmail = validPayload.Email;
                // Console.WriteLine(validEmail);

                var user_email = _context.Users.Where(u => u.email == validEmail).FirstOrDefault();

                // if (user_email == null)
                // {
                //     Console.WriteLine("this is the current email", user_email);
                // }

                if (user_email == null)
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
                       

                        _context.Users.Add(user);;
                        await _context.SaveChangesAsync();
                        return Ok(user.firstName + "is created");
                    }
                    catch (Exception ex)
                    {
                        return NotFound("hitting the error for trying to create a user");

                    }
                }

                // outside return statment
                return Ok("this is the payload" + validPayload);
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
