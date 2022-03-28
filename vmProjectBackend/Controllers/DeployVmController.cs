<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Mvc;
>>>>>>> auth-ebe
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
<<<<<<< HEAD

=======
using vmProjectBackend.Services;
>>>>>>> auth-ebe

namespace vmProjectBackend.Controllers
{

<<<<<<< HEAD
[Route("api/[controller]")]
    [ApiController]
    public class DeployVmController : ControllerBase
    {
        private readonly VmContext _context;
        public IConfiguration Configuration { get; }
        private readonly IHttpClientFactory _httpClientFactory;
        public DeployVmController(VmContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
=======
    [Route("api/[controller]")]
    [ApiController]
    public class DeployVmController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public IConfiguration Configuration { get; }
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Authorization _auth;
        public DeployVmController(DatabaseContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
>>>>>>> auth-ebe
        {
            _context = context;
            _httpClientFactory = httpClientFactory;

            Configuration = configuration;
<<<<<<< HEAD
            
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
                //Query the folder name in order to use it when we do the post
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
                var template_id = listOfCourse[0].template_id; 
                // Create a session token
                var httpClient = _httpClientFactory.CreateClient();
                string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
=======
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
                                     enrollment_id = usr.UserSectionRoleId,
                                     folder = c.Folder
                                  }).ToArray();
                var template_id = courseList[0].template_id;

                // Create a session token
                var httpClient = _httpClientFactory.CreateClient();
                string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
                //Adding headers
                httpClient.DefaultRequestHeaders.Add("Authorization", base64);
>>>>>>> auth-ebe
                var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
                Console.WriteLine(tokenResponse);
                string tokenstring = " ";
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                //Taking quotes out of the tokenstring variable s = s.Replace("\"", "");
<<<<<<< HEAD
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
                         name = HttpContext.User.Identity.Name,
                         placement = new Placement 
                            {
                                folder = listOfCourse[0].folder,  
                                
                                resource_pool = resourcePool

                            }
                         };
                    var deployResult = JsonConvert.SerializeObject(deploy);

                    // var Strcontent = new StringContent(deployResult);

                    // return Ok(deploy);
                  
                    var deployContent = new StringContent(deployResult, Encoding.UTF8, "application/json");
                    // return Ok(deployResult);

                    var postResponse = await httpClient.PostAsync($"https://vctr-dev.citwdd.net/api/vcenter/vm-template/library-items/{template_id}?action=deploy", deployContent);
                    // return Ok (postResponse.RequestMessage);  

                    // postResponse.EnsureSuccessStatusCode();

                     // Deleting Session to avoid data redundancy in Vsphere

                    var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");
                    // return Ok("vm created and session deleted");

                    VmTable vmTable = new VmTable();

                    vmTable.VmFolder = listOfCourse[0].folder;
                    vmTable.VmName = listOfCourse[0].student_name;
                    vmTable.VmResourcePool = resourcePool;
                    
                    _context.VmTables.Add(vmTable);
                   await _context.SaveChangesAsync();
                return Ok("something was sent");
                }
                return Ok("token response wasn't successful");
=======
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
>>>>>>> auth-ebe
            }
            return Unauthorized("You are not Authorized and this is not a student");
        }

<<<<<<< HEAD
         [HttpGet("resource-pool")]
         public async Task<ActionResult<IEnumerable<Pool>>> GetPools()
         {
=======
        [HttpGet("resource-pool")]
        public async Task<ActionResult<IEnumerable<Pool>>> GetPools()
        {
>>>>>>> auth-ebe
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
<<<<<<< HEAD
                //Scape characters functions to filter the new header results
                tokenstring = tokenstring.Replace("\"", "" );
                tokenstring = tokenstring.Replace("{", "" );
                tokenstring = tokenstring.Replace("value:", "" );
                tokenstring = tokenstring.Replace("}", "" );
=======

                //Scape characters functions to filter the new header results
                tokenstring = tokenstring.Replace("\"", "");
                tokenstring = tokenstring.Replace("{", "");
                tokenstring = tokenstring.Replace("value:", "");
                tokenstring = tokenstring.Replace("}", "");
>>>>>>> auth-ebe
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                //Call resource-pool uri in vsphere
                var response = await httpClient.GetAsync("https://vctr-dev.citwdd.net/api/vcenter/resource-pool");
                //Turn these objects responses into a readable string
<<<<<<< HEAD
                string poolResponseString = await response.Content.ReadAsStringAsync(); 
                // turn the readable string into a json collection
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
                tokenstring = tokenstring.Replace("\"", "" );
                tokenstring = tokenstring.Replace("{", "" );
                tokenstring = tokenstring.Replace("value:", "" );
                tokenstring = tokenstring.Replace("}", "" );
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

             var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");             
            return Ok("Session Deleted");
            //This endpoint is just for testing purposes, we should delete it later

        }
 
=======
                string poolResponseString = await response.Content.ReadAsStringAsync();
                // Pool poolResponse = JsonConvert.DeserializeObject<Pool>(poolResponseString);
                List<String> poolResponse = poolResponse = JsonConvert.DeserializeObject<List<String>>(poolResponseString);
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

>>>>>>> auth-ebe
    }


}
<<<<<<< HEAD


=======
>>>>>>> auth-ebe
