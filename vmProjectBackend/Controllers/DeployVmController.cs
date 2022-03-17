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
        public async Task<ActionResult<VmDetail>> PostVmTable(string enrollment_id)
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
                                    .Select(e => new
                                    {
                                        student_name = $"{e.User.firstName} {e.User.lastName}",
                                        course_name = e.Course.CourseName,
                                        course_id = e.CourseID,
                                        template_id=e.Course.TemplateVm,
                                        course_semester = e.semester,
                                        enrollment_id = e.EnrollmentID
                                    })
                                    .ToArrayAsync();
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
                     string resourcePool = Configuration.GetConnectionString("Resource_Pool");
                    // Create vm with the information we have in vsphere
                    var deploy = new Deploy
                        {
                         name = useremail = HttpContext.User.Identity.Name,
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

    }

}
