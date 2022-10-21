using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using vmProjectBFF.DTO;
using vmProjectBFF.Models;
using vmProjectBFF.Services;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VmTableController : BffController
    {

        public VmTableController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<VmTableController> logger,
            IVCenterRepository vCenter)
            : base(
                  authorization: authorization,
                  backend: backend,
                  canvas: canvas,
                  configuration: configuration,
                  httpClientFactory: httpClientFactory,
                  httpContextAccessor: httpContextAccessor,
                  logger: logger,
                  vCenter: vCenter)
        {
        }
        [HttpGet("instances")]
        public async Task<ActionResult<dynamic>> GetInstances()
        {
            try
            {
                User user = _authorization.GetAuth("user");
                if (user is not null)
                {
<<<<<<< HEAD
                    return _backend.GetInstancesByUserId(user.UserId);
=======
                    return _backend.GetInstancesByUserId(1017);
>>>>>>> main
                }
                else
                {
                    return Forbid();
                }
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }

        //GET: api/vmtable/templates
        [HttpGet("templates/all")]
        public async Task<ActionResult<IEnumerable<string>>> GetTemplates(string libraryId)
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    return Ok(_vCenter.GetTemplatesByContentLibraryId(libraryId));
                }
                else
                {
                    return Forbid();
                }
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);

            }
        }
    }
}