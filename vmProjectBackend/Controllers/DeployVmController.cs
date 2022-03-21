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


namespace vmProjectBackend.Controllers
{

[Route("api/[controller]")]
    [ApiController]
    public class DeployVmController : ControllerBase
    {
        private readonly VmContext _context;
        public IConfiguration Configuration { get; }
        private readonly IHttpClientFactory _httpClientFactory;
        public DeployVmController(VmContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;

            Configuration = configuration;
        }

                //Connect our API to a second API that creates our vms 
        [HttpPost()]
        public async Task<ActionResult<VmDetail>> PostVmTable(Guid enrollment_id)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if the user is a student
            var user_student = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Student")
                            .FirstOrDefault();
            if (user_student != null)
            {
                //Query the student name in order to use it as the vm's name
                 var listOfCourse = await _context.Enrollments
                                    .Include(c => c.Course)
                                    .Where(s => s.UserId == user_student.UserID)
                                    .Where(t => t.EnrollmentID == enrollment_id)
                                    .Select(e => new
    
                                    {
                                        student_name = $"{e.User.firstName} {e.User.lastName}",
                                        course_name = e.Course.CourseName,
                                        course_id = e.CourseID,
                                        template_id=e.Course.TemplateVm,
                                        course_semester = e.semester,
                                        enrollment_id = e.EnrollmentID,
                                        folder = e.Course.Folder
                                    })
                                    .ToArrayAsync();
                                    // return Ok(listOfCourse);
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
                tokenstring = tokenstring.Replace("\"", "" );
                tokenstring = tokenstring.Replace("{", "" );
                tokenstring = tokenstring.Replace("value:", "" );
                tokenstring = tokenstring.Replace("}", "" );
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                if (tokenResponse.IsSuccessStatusCode)
                {
                     string resourcePool = Configuration["Resource_Pool"];
                    // Create vm with the information we have in vsphere
                    
                    var deploy = new Deploy
                        {
                         name = useremail = HttpContext.User.Identity.Name,
                         placement = new Placement 
                            {
                                folder = listOfCourse[0].folder,  
                                
                                resource_pool = resourcePool

                            }
                         };
                    var deployResult = JsonConvert.SerializeObject(deploy);

                    // var content = new StringContent(deployResult);

                    var content = new StringContent(deployResult, Encoding.UTF8, "application/json");

                    var postResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/api/vcenter/vm-template/library-items/b4f40b57-21e5-48c2-9fdf-03337fe8d9c1?action=deploy", content);

                    postResponse.EnsureSuccessStatusCode();

                     // Deleting Session to avoid data redundancy in Vsphere

                    var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");

                         
                    return Ok("vm created and session deleted");
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
                tokenstring = tokenstring.Replace("\"", "" );
                tokenstring = tokenstring.Replace("{", "" );
                tokenstring = tokenstring.Replace("value:", "" );
                tokenstring = tokenstring.Replace("}", "" );
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
                tokenstring = tokenstring.Replace("\"", "" );
                tokenstring = tokenstring.Replace("{", "" );
                tokenstring = tokenstring.Replace("value:", "" );
                tokenstring = tokenstring.Replace("}", "" );
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

             var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");
             
            return Ok("Session Deleted");

        }

         }


     }


