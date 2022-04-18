using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using Microsoft.Extensions.Logging;
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
using Microsoft.AspNetCore.Authorization;

namespace vmProjectBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeployVmController : ControllerBase
    {
        private readonly Authorization _auth;
        private readonly Backend _backend;
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DeployVmController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeployVmController(
            DatabaseContext context,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<DeployVmController> logger)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _auth = new(_backend, _context, _logger);
            _httpClientFactory = httpClientFactory;
        }

        //Connect our API to a second API that creates our vms 
        [HttpPost()]
        public async Task<ActionResult<VmDetail>> PostVmTable(string enrollment_id)
        {
            string userEmail = HttpContext.User.Identity.Name;
            // check if it is student
            User studentUser = _auth.getAuth("user");
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
                                     enrollment_id = usr.UserSectionRoleId,
                                     folder = c.Folder
                                  }).ToArray();
                var template_id = courseList[0].template_id;

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
                    string resourcePool = _configuration["Resource_Pool"];
                    // Create vm with the information we have in vsphere
                    var deploy = new Deploy
                    {

                        name = HttpContext.User.Identity.Name,
                        placement = new Placement
                        {
                            folder = courseList[0].folder,

                            resource_pool = resourcePool

                        }
                    };
                    var deployResult = JsonConvert.SerializeObject(deploy);

                    // var content = new StringContent(deployResult);

                    var deployContent = new StringContent(deployResult, Encoding.UTF8, "application/json");

                    // var content2 = new StringContent(content, Encoding.UTF8, "application/json");

                    // return Ok(content2);

                    var postResponse = await httpClient.PostAsync($"https://vctr-dev.citwdd.net/api/vcenter/vm-template/library-items/{template_id}?action=deploy", deployContent);
                    
                    var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");
                    //  var content2 = await postResponse.Content.ReadAsStringAsync();

                    //  var createdCompany = JsonSerializer.Deserialize<DeployDto>(content, _options);
                    VmTable vmTable = new VmTable();

                    vmTable.VmFolder = courseList[0].folder;
                    vmTable.VmName = courseList[0].student_name;
                    vmTable.VmResourcePool = resourcePool;

                    _context.VmTables.Add(vmTable);
                    await _context.SaveChangesAsync();

                    return Ok("something was sent");
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
                var objResponse = JsonConvert.DeserializeObject<List<Pool>>(poolResponseString);
                return Ok(objResponse);
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
