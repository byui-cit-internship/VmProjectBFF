using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using vmProjectBackend.DTO;
using System.Net.Http;
using Newtonsoft.Json;
namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VmTableController : ControllerBase

    {

        private readonly VmContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public VmTableController(VmContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/VmTable
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VmTable>>> GetVmTables()
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                return await _context.VmTables.ToListAsync();
            }
            return Unauthorized("You are not Authorized and this is not a professor");
        }

        //GET: api/vmtable/templates
        [HttpGet("templates/all")]
        public async Task<ActionResult<IEnumerable<string>>> GetTemplates(string libraryId) 
        {
            string useremail = HttpContext.User.Identity.Name;
            //check if it is a professor
            var user_prof = _context.Users
                              .Where(p => p.email == useremail && p.userType == "Professor")
                              .FirstOrDefault();
            Console.WriteLine("hola, do something" + user_prof.userType);

            if (user_prof != null)
            {

                try
                {

                    // Creating the client request and setting headers
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


                    }
                    httpClient.DefaultRequestHeaders.Remove("Authorization");
                    //we are removing the basic auth because it require a new authorization
                    httpClient.DefaultRequestHeaders.Add("vmware-api-session-id", tokenstring);
                    // contains our base Url where templates were added in vcenter
                    // This URL enpoint gives a list of all the Templates we have in our vcenter 
                    List<Template> templates = new List<Template>();

                    var response = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item?library_id={libraryId}");
                    Console.WriteLine($" response to the second call {response}");

                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("template response" + responseString);
                    List<String> templateIds = templateIds = JsonConvert.DeserializeObject<List<String>>(responseString);


                    //call Api, convert it to templates, and get the list of templates
                    foreach (string templateId in templateIds)
                    {
                        var response2 = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item/" + templateId);
                        Console.WriteLine($"Second response {response2}");

                       
                        string response2String = await response2.Content.ReadAsStringAsync();
                        Template template = JsonConvert.DeserializeObject<Template>(response2String);       
                        templates.Add(template);

                  

                        

                    }


                    if (templates != null)
                    {
                        return Ok(templates);
                    }
                    else
                    {
                        return NotFound("Failed calling");
                    }
                }
                catch
                {
                    return Problem("crash returning templates!");

                }

            }
            return Unauthorized("You are not Authorized and is not a professor");

        }
        // GET: api/VmTable/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VmTable>> GetVmTable(Guid id)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();
            if (user_prof != null)
            {
                var vmTable = await _context.VmTables.FindAsync(id);

                if (vmTable == null)
                {
                    return NotFound();
                }
                return Ok(vmTable);
            }
            return Unauthorized();
        }
        // patch a vm template
        [HttpPatch("update/{id}")]
        public async Task<ActionResult> PartialUpdate(Guid id, JsonPatchDocument<VmTable> patchDoc)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();
            if (user_prof != null)
            {
                var orginalVm = await _context.VmTables.FirstOrDefaultAsync(c => c.VmTableID == id);

                if (orginalVm == null)
                {
                    return NotFound();
                }

                patchDoc.ApplyTo(orginalVm, ModelState);
                await _context.SaveChangesAsync();

                return Ok(orginalVm);
            }
            return Unauthorized();

        }
        // POST: Api/Utilization
        //professor creates a template
        [HttpPost("utilization/professor")]
        public async Task<ActionResult<VmUtilization>> PostVmRecord(VmUtilization vmUtilization){
            string useremail = HttpContext.User.Identity.Name;
            //check if it is a professor
            var user_professor = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();
            //Save what we have in vmUtilization model into our database
                            if (user_professor != null)
                            {
            // Saves the information into our db                    
                _context.VmUtilizations.Add(vmUtilization);
                await _context.SaveChangesAsync();

                return Ok(vmUtilization); 
                
                            }  

                            return Unauthorized();  
        }

        [HttpGet("utilization")]
        public async Task<ActionResult<IEnumerable<VmUtilization>>> VmUtilization()
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                return await _context.VmUtilizations.ToListAsync();
            }
            return Unauthorized("You are not Authorized and you are not a professor");
        }
        [HttpGet("utilization/student")]
        public async Task<ActionResult<IEnumerable<VmUtilization>>>GetUtilizations()
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is student
            var user_student = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Student")
                            .FirstOrDefault();
            // students are able to store their vm's details 
            if (user_student != null)
            {
                return await _context.VmUtilizations.ToListAsync();
            }
            return Unauthorized("You are not Authorized and you are not a professor");
        }
        //Make a request to create a vm in Vcenter
        [HttpPost("virtualmachine")]
        public async Task<ActionResult<VmUtilization>> PostVm(VmUtilization vmUtilization)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is student
            var user_student = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Student")
                            .FirstOrDefault();

            if (user_student != null) {
                var httpClient = _httpClientFactory.CreateClient();
            // Create a vm by calling Vcenter
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x");
            // Send vm detalis to Vcenter
                var response3 = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/vcenter/vm", null);
                string response3String = await response3.Content.ReadAsStringAsync();
                return Ok(vmUtilization); 
                
            }
            else {
                return Unauthorized();
            }

        }        
        // POST: api/VmTable
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost()]
        public async Task<ActionResult<VmTable>> PostVmTable(VmTable vmTable)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                _context.VmTables.Add(vmTable);
                await _context.SaveChangesAsync();

                return Ok(vmTable);
            }
            return Unauthorized();
        }

        // DELETE: api/VmTable/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVmTable(Guid id)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                var vmTable = await _context.VmTables.FindAsync(id);
                if (vmTable == null)
                {
                    return NotFound();
                }
                _context.VmTables.Remove(vmTable);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            return Unauthorized();
        }
        // PUT: api/VmTable/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutVmTable(Guid id, VmTable vmTable)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                if (id != vmTable.VmTableID)
                {
                    return BadRequest();
                }

                _context.Entry(vmTable).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VmTableExists(id))
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
            return Unauthorized();
        }
        private bool VmTableExists(Guid id)
        {
            return _context.VmTables.Any(e => e.VmTableID == id);
        }
    }
}
