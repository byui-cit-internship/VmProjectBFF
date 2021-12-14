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
    public class LiveprobeController : ControllerBase
    {
        private readonly VmContext _context;

        public LiveprobeController(VmContext context)
        {
            _context = context;
        }

        // GET: api/Liveprobe
        [HttpGet]
        public async Task<ActionResult<User>> GetProbe()
        {
            return Ok();
        }


    }
}
