using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;

namespace vmProjectBackend.Controllers
{
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
            return await _context.VmTables.ToListAsync();
        }

        // GET: api/VmTable/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VmTable>> GetVmTable(long id)
        {
            var vmTable = await _context.VmTables.FindAsync(id);

            if (vmTable == null)
            {
                return NotFound();
            }

            return vmTable;
        }

        // PUT: api/VmTable/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVmTable(long id, VmTable vmTable)
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

        // POST: api/VmTable
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VmTable>> PostVmTable(VmTable vmTable)
        {
            _context.VmTables.Add(vmTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVmTable", new { id = vmTable.VmTableID }, vmTable);
        }

        // DELETE: api/VmTable/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVmTable(long id)
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
        [HttpGet("vmdetails/{id}")]
        public async Task<IActionResult> VmDetails(int id)
        {

            var vmDetail = await _context.VmTables
            .Include(s => s.VmTableCourses)
            .ThenInclude(e => e.Course.Enrollments)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.VmTableID == id);

            // if the user is not found
            if (vmDetail == null)
            {
                return NotFound();
            }

            return Ok(vmDetail);
        }

        private bool VmTableExists(long id)
        {
            return _context.VmTables.Any(e => e.VmTableID == id);
        }
    }
}
