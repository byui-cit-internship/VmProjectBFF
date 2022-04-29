using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// this endpoint is needed for your cloudbuild-dev.yaml file for the livenessProbe.
// may want to review whether you need to add the [Attorize] declarator.
namespace vmProjectBFF.Controllers
{
    [Route("/")]
    [ApiController]
    public class LiveprobeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LiveprobeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // For kubernetes to indicate pod health.
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetProbe()
        {
            _httpContextAccessor.HttpContext.Session.SetInt32("setCookie", 1);
            return Ok();
        }
    }
}
