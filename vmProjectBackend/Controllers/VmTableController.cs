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

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VmTableController : ControllerBase
    {
        private readonly VmContext _context;

        public VmTableController(VmContext context)
        {
            _context = context;
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

                return vmTable;
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

        private bool VmTableExists(Guid id)
        {
            return _context.VmTables.Any(e => e.VmTableID == id);
        }

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
    }
}
