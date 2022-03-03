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

namespace vmProjectBackend.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class CreateVm : ControllerBase {
        private readonly VmContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

         public CreateVm(VmContext context)
        {
            _context = context;
        }
    //Connect our API to a second API that creates our vms 
        [HttpPost("createvm")]
        public async Task<ActionResult<VmTable>> GetVmTable(Guid id) {
            
        string useremail = HttpContext.User.Identity.Name;
            // check if it is a student
            var user_student = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Student")
                            .FirstOrDefault();
            if (user_student != null)
            {
            //Create a session token
             var httpClient = _httpClientFactory.CreateClient();
                    string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
                    Console.WriteLine(base64);

                    httpClient.DefaultRequestHeaders.Add("Authorization", base64);

                    var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/api/session", null);
                    Console.WriteLine(tokenResponse);
                    string tokenstring = " ";
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                        //Taking quotes out of the tokenstring variable s = s.Replace("\"", "");
                        tokenstring = tokenstring.Replace("\"", "");

                        Console.WriteLine($"it was sucessfull {tokenstring}");

                var vmTable = await _context.VmTables.FindAsync(id);

                if (vmTable == null)
                {
                    return NotFound();
                }
                return Ok(vmTable);
            }
            return Unauthorized();
        }

        //Working here
        return Ok();
    
    }

    }
}

           

    

