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

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.Net.Http.Headers;

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
            return Unauthorized("You are not Authorized and is not a professor");
        }

        //GET: api/vmtable/templates
         [HttpGet("templates/all")] 
         public async Task<ActionResult<IEnumerable<string>>> GetTemplates()
         {
         string useremail = HttpContext.User.Identity.Name;
         //check if it is a professor
          var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
               
                try{ 
                    string userName = "api-test@vsphere.local";
                    string password = "wnD<Dz[HZW@25{]q";
                    var httpClient = _httpClientFactory.CreateClient();
                     string basic = userName + ":" + password;

                      var base64EncodedBytes = System.Convert.FromBase64String(basic); 
                      var base64EncodedString = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                      
                 httpClient.DefaultRequestHeaders.Add("authorization", base64EncodedString);

                var tokenResponse = await httpClient.GetAsync("https://vctr-dev.citwdd.net/api/session");
                string tokenstring = " ";
                if (tokenResponse.IsSuccessStatusCode)
                {
                 tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"it was sucessfull {tokenstring}");
                }
                
                httpClient.DefaultRequestHeaders.Add("vmware-api-session-id", tokenstring);
                // contains our base Url where templates were added in vcenter
                // This URL enpoint gives a list of all the Templates we have in our vcenter 
                var response = await httpClient.GetAsync($"https://vctr.citwdd.net/api/vcenter/vm-template/library-items/");

                string responseString = await response.Content.ReadAsStringAsync();
     
                if (response.IsSuccessStatusCode)
                 {
                    return Ok(response);
                }
                else {
                    return NotFound("Failed calling");
                }
                }
                catch
                {
                    Ok("You failed");
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
