using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;

namespace vmProjectBackend.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class CreateVmController : ControllerBase {
        private readonly VmContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
         public CreateVmController(VmContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
    //Connect our API to a second API that creates our vms 
        [HttpPost()]
        public async Task<ActionResult<VmDetail>> PostVmTable(VmDetail vmDetail) {
            
        string useremail = HttpContext.User.Identity.Name;
            // check if it is a student
            var user_student = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Student")
                            .FirstOrDefault();
            if (user_student != null)
            { 
            // return Ok("hit");
            // Create a session token
                    var httpClient = _httpClientFactory.CreateClient();
                   string base64 = "YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
                   var EncodedAuthentication = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(base64));
                    Console.WriteLine(base64);
                    // httpClient.DefaultRequestHeaders.Add("Authorization", base64);

                    // ("Authorization", String.Format("Basic {0}", base64));
                     httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {EncodedAuthentication}");

                    var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
                    Console.WriteLine(tokenResponse);
                   string tokenstring = " ";
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                        //Taking quotes out of the tokenstring variable s = s.Replace("\"", "");
                        tokenstring = tokenstring.Replace("\"", "");
                        Console.WriteLine($"it was sucessfull {tokenstring}");
                        //Create vm with the information we have in vsphere
                         _context.VmDetails.Add(vmDetail);
                        return Ok("here session");
                    }
                    return Ok("Done");
            }
            return Unauthorized("You are not Authorized and this is not a student");
                }
    
    }
}

           

    

