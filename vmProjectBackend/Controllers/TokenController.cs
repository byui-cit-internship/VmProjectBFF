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

        // POST: api/Token
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        // private const string GoogleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";

        // private async Task<GoogleJsonWebSignature.Payload> ValidateIdTokenAndGetUserInfo(string Id_token)
        // {
        //     if (string.IsNullOrWhiteSpace(Id_token))
        //     {
        //         return null;
        //     }

        //     try
        //     {
        //         return await GoogleJsonWebSignature.ValidateAsync(Id_token);
        //     }
        //     catch (Exception exception)
        //     {
        //         // _Logger.LogError(exception, $"Error calling ValidateIdToken in GoogleAuthenticateHttpClient");
        //         return null;
        //     }
        // }
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
                Console.WriteLine(validEmail);


                // User user = new User();
                
                // user.email = validPayload.Email;
                // user.firstName = validPayload.name
                // user.
                // var user_email = _context.Users.ToList();

                var user_email = _context.Users.Where(u => u.email == validEmail).FirstOrDefault();

                if (user_email == null)
                {
                    /*

                    string fullName = validPayload.name
                    // split the full name
                    string[] names  = fullName.Split(' ');
                    



                    
                    
                    
                    */
                    // save that current user to the data base
                    //  _context.Users.Add();
                    return NotFound("User not in database");

                }

                Console.WriteLine(user_email.firstName);
                // return Ok(user_email.userType);
                return Ok(validPayload);




                // if(user_email null){

                // }



                // Console.WriteLine(user.email);

                //    List of users
                //    for loop into all those user:
                //    if email is int database:
                //      if it the user type eqaul to type- ProducesDefaultResponseTypeAttribute then am going to send type pro
                //      else SendFileFallback type student

                //     else
                //         PutToken them in the database


                //...
            }
            catch (Exception ex)
            {
                //...
                return NotFound(ex);
            }
            // _context.Tokens.Add(token);

            // try
            // {
            //     await _context.SaveChangesAsync();
            // }
            // catch (DbUpdateException)
            // {
            //     if (TokenExists(token.ID))
            //     {
            //         return Conflict();
            //     }
            //     else
            //     {
            //         throw;
            //     }
            // }

            // return CreatedAtAction("GetToken", new { id = token.ID }, token);
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
