using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using vmProjectBackend.DTO;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace vmProjectBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CreateVmController : ControllerBase
    {
        private readonly VmContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public CreateVmController(VmContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        //Connect our API to a second API that creates our vms 
        [HttpPost()]
        public async Task<ActionResult<VmDetail>> PostVmTable(VmDetail vmDetail)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a student
            var user_student = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Student")
                            .FirstOrDefault();
            if (user_student != null)
            {
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
                    // Create vm with the information we have in vsphere
                    _context.VmDetails.Add(vmDetail);
                    return Ok("here session");
                }
            }
            return Unauthorized("You are not Authorized and this is not a student");
        }

        [HttpGet("libraries")]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            //Open uri communication
            var httpClient = _httpClientFactory.CreateClient();
            // Basic authentication in base64
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
            Console.WriteLine(tokenResponse);
            if (tokenResponse.IsSuccessStatusCode)
            {
                string tokenstring = " ";
                //Turn this object into a readable string
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();

                //Scape characters functions to filter the new header results
                tokenstring = tokenstring.Replace("\"", "" );
                tokenstring = tokenstring.Replace("{", "" );
                tokenstring = tokenstring.Replace("value:", "" );
                tokenstring = tokenstring.Replace("}", "" );
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                //Make a list of Library objects
                List<Library> libraries = new List<Library>();
                //Call library uri in vsphere
                var responseLibraryIds = await httpClient.GetAsync("https://vctr-dev.citwdd.net/api/content/local-library");
                //Turn these objects responses into a readable string
                string responseStringLibraries = await responseLibraryIds.Content.ReadAsStringAsync();
                //Make a list of library id's   
                List<String> libraryIds = JsonConvert.DeserializeObject<List<String>>(responseStringLibraries);
                //Create a list using our Dto          
                foreach (string Id in libraryIds)
                {
                    // return Ok(libraryIds);
                    var libraryresponse = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/local-library/" + Id);
                    Console.WriteLine($"Second response {libraryresponse}");
                    string response2String = await libraryresponse.Content.ReadAsStringAsync();
                    Library library = JsonConvert.DeserializeObject<Library>(response2String);
                    libraries.Add(library);                   
                }
                return Ok(libraries);
                // if (libraries != null)
                // {
                    
                //     httpClient.DefaultRequestHeaders.Add("Authorization", base64);
                //     httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

                //     var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");
                //     return Ok("Session was deleted here");
                // }
            }
            return Unauthorized("here");
        }   
        // [HttpDelete]     

        // public async Task<ActionResult<IEnumerable<Library>>> DeleteSession()
        // {
        // var httpClient = _httpClientFactory.CreateClient();

        // string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
        // // Adding headers to this call
        // httpClient.DefaultRequestHeaders.Add("Authorization", base64);
        // httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

        // var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");
        // return Ok("Session was deleted here");
        // }
    }
}