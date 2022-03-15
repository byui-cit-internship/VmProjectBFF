using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.DAL;

// this endpoint is needed for your cloudbuild-dev.yaml file for the livenessProbe.
// may want to review whether you need to add the [Attorize] declarator.
namespace vmProjectBackend.Controllers
{
    [Route("/")]
    [ApiController]
    public class LiveprobeController : ControllerBase
    {
        // For kubernetes to indicate pod health.
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetProbe()
        {
            return Ok();
        }
    }
}
