using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using vmProjectBackend.DTO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using vmProjectBackend.Services;

namespace vmProjectBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DeployVmController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public IConfiguration Configuration { get; }
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Authorization _auth;
        public DeployVmController(DatabaseContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;

            Configuration = configuration;
            _auth = new Authorization(_context);
        }

        //Connect our API to a second API that creates our vms 
        [HttpPost()]
        public async Task<ActionResult<VmDetail>> PostVmTable(string enrollment_id)
        {
            string userEmail = HttpContext.User.Identity.Name;
            // check if it is student
            User studentUser = _auth.getUser(userEmail);
            // students are able to store their vm's details

            if (studentUser != null)
            {
                //Query the student name in order to use it as the vm's name
                var courseList = (from u in _context.Users
                                  join usr in _context.UserSectionRoles
                                  on u.UserId equals usr.UserId
                                  join s in _context.Sections
                                  on usr.SectionId equals s.SectionId
                                  join c in _context.Courses
                                  on s.CourseId equals c.CourseId
                                  where u.UserId == studentUser.UserId
                                  select new
                                  {
                                     student_name = $"{u.FirstName} {u.LastName}",
                                     course_name = c.CourseName,
                                     course_id = c.CourseId,
                                     template_id = c.TemplateVm,
                                     course_semester = c.Semester,
                                     enrollment_id = usr.UserSectionRoleId
                                  });

                
                // Create a session token
                var httpClient = _httpClientFactory.CreateClient();
                string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
                //Adding headers
                httpClient.DefaultRequestHeaders.Add("Authorization", base64);
                var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
                Console.WriteLine(tokenResponse);
                string tokenstring = " ";
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                //Taking quotes out of the tokenstring variable s = s.Replace("\"", "");
                tokenstring = tokenstring.Replace("\"", "");
                tokenstring = tokenstring.Replace("{", "");
                tokenstring = tokenstring.Replace("value:", "");
                tokenstring = tokenstring.Replace("}", "");
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                if (tokenResponse.IsSuccessStatusCode)
                {
                    string resourcePool = Configuration["Resource_Pool"];
                    // Create vm with the information we have in vsphere
                    var deploy = new Deploy
                    {
                        
                        placement = new Placement
                        {
                            folder = "group-v3044",
                            resource_pool = resourcePool

                        }
                    };
                    var deployResult = JsonConvert.SerializeObject(deploy);

                    // var content = new StringContent(deployResult);

                    var content = new StringContent(deployResult, Encoding.UTF8, "application/json");

                    // var content2 = new StringContent(content, Encoding.UTF8, "application/json");

                    // return Ok(content2);

                    var postResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/api/vcenter/vm-template/library-items/b4f40b57-21e5-48c2-9fdf-03337fe8d9c1?action=deploy", content);

                    postResponse.EnsureSuccessStatusCode();

                    //  var content2 = await postResponse.Content.ReadAsStringAsync();

                    //  var createdCompany = JsonSerializer.Deserialize<DeployDto>(content, _options);

                    return Ok("vm created");
                }
                return Ok("here session");
            }
            return Unauthorized("You are not Authorized and this is not a student");
        }

        [HttpGet("resource-pool")]
        public async Task<ActionResult<IEnumerable<Pool>>> GetPools()
        {
            //Open uri communication
            var httpClient = _httpClientFactory.CreateClient();
            // Basic authentication in base64
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
            if (tokenResponse.IsSuccessStatusCode)
            {
                string tokenstring = " ";
                //Turn this object into a readable string
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();

                //Scape characters functions to filter the new header results
                tokenstring = tokenstring.Replace("\"", "");
                tokenstring = tokenstring.Replace("{", "");
                tokenstring = tokenstring.Replace("value:", "");
                tokenstring = tokenstring.Replace("}", "");
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                //Call resource-pool uri in vsphere
                var response = await httpClient.GetAsync("https://vctr-dev.citwdd.net/api/vcenter/resource-pool");
                //Turn these objects responses into a readable string
                string poolResponseString = await response.Content.ReadAsStringAsync();
                // Pool poolResponse = JsonConvert.DeserializeObject<Pool>(poolResponseString);
                List<String> poolResponse = poolResponse = JsonConvert.DeserializeObject<List<String>>(poolResponseString);
                return Ok(poolResponse);
                // string folders2 = await folders.Content.ReadAsStringAsync();
                // //Create a list using our Dto                         
                // return Ok(folders2);
            }
            return Unauthorized("here");

        }

        [HttpDelete()]

        public async Task<IActionResult> DeleteSession()
        {
            // Create a session token
            var httpClient = _httpClientFactory.CreateClient();
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
            string tokenstring = " ";
            tokenstring = await tokenResponse.Content.ReadAsStringAsync();
            //Scape characters functions to filter the new header results
            tokenstring = tokenstring.Replace("\"", "");
            tokenstring = tokenstring.Replace("{", "");
            tokenstring = tokenstring.Replace("value:", "");
            tokenstring = tokenstring.Replace("}", "");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

            var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");

            return Ok("Session Deleted");

        }

    }


}
