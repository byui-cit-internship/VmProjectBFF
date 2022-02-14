using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;

// this endpoint is needed for your cloudbuild-dev.yaml file for the livenessProbe.
// may want to review whether you need to add the [Attorize] declarator.
namespace vmProjectBackend.Controllers
{
    [Route("/")]
    [ApiController]
    public class LiveprobeController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public LiveprobeController(DatabaseContext context)
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
