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
 

// https://localhost:5001/api/token/eyJhbGciOiJSUzI1NiIsImtpZCI6ImQ0ZTA2Y2ViMjJiMDFiZTU2YzIxM2M5ODU0MGFiNTYzYmZmNWE1OGMiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJhY2NvdW50cy5nb29nbGUuY29tIiwiYXpwIjoiMTA0Mzc0NzMxMzM2MS0wcm01ZWJ0ZjJ0cmpmazZlcWdnNWl2YXM4dWJzYW43di5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsImF1ZCI6IjEwNDM3NDczMTMzNjEtMHJtNWVidGYydHJqZms2ZXFnZzVpdmFzOHVic2FuN3YuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDQ2NzI3MDc4NTY2MDIzNTkyMDUiLCJlbWFpbCI6InRub2xhc2NvNTRAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImF0X2hhc2giOiJDTzJPc3VsdnM5Z1BCaDBGa19DQ2p3IiwibmFtZSI6IlRhbW15IE5vbGFzY28iLCJwaWN0dXJlIjoiaHR0cHM6Ly9saDMuZ29vZ2xldXNlcmNvbnRlbnQuY29tL2EtL0FPaDE0R2l6aHc1WVpPalBhVTBwVjY3V0dILTFRV2h3Q2dCNXliUFFxUDlRQUE9czk2LWMiLCJnaXZlbl9uYW1lIjoiVGFtbXkiLCJmYW1pbHlfbmFtZSI6Ik5vbGFzY28iLCJsb2NhbGUiOiJlbiIsImlhdCI6MTYzODIyNDgzNiwiZXhwIjoxNjM4MjI4NDM2LCJqdGkiOiJlNWFmOTMyNGZjY2JjMjVjN2RkM2ZhZThmNmQxZjkyNTU3MjNjMjgxIn0.eu0NUHjcESF7epbbV5jQVAX1TMQUcxjgX17ws-h7qAdvSpmoiq743CSoff_KDSaeVvwFLYO4vZZPXD578hEyH9YvDa2QaFUCx-RT18GU54LlwxhlJTzaf8AWWNGE-IgBWiVTjKeM627j9Anv9rugsF1PwWdkcVAn7WCQcOmHKc8GoK7AT1PDHSL128gPs-xMIC__OgQo2gSQGcjywwSi0jbpjWLLq2P7aGek-OgaYrdGsRK2SpC0r6F5etBzGPlrFcFMY0QHrTBhjC22Akx19XTKPPqRfeUmO3yXzU8bthj5tbWCpfu1b3Xo0feAp8DMcQwBLbcOzQLAZm27Vbf_5g

        [HttpGet("{token}")]
        public async Task<ActionResult<Token>> PostToken(string token)
        {

            
            // GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(Id_token);
            // ValidateIdTokenAndGetUserInfoId(Id_token);

            try
            {
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(token);

                string validEmail = validPayload.Email;
                // Console.WriteLine(validEmail);

                var user_detail = _context.Users.Where(u => u.email == validEmail).FirstOrDefault();

                // if (user_email == null)
                // {
                //     Console.WriteLine("this is the current email", user_email);
                // }

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
                        return Ok(user);
                    }
                    catch (Exception ex)
                    {
                        return NotFound("hitting the error for trying to create a user");

                    }
                }

                // outside return statment
                return Ok(validPayload);
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
