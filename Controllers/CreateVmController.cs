using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;
using vmProjectBFF.Models;
using vmProjectBFF.Services;

namespace vmProjectBFF.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreateVmController : BffController
    {

        public CreateVmController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CreateVmController> logger,
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


        /**
         * <summary>
         * Gets a list of libraries from vsphere
         * </summary>
         * <returns>A list of vsphere libraries, where each list member contains the vsphere id and name of the library.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/CreateVm/libraries
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      GET /api/CreateVm/libraries
         *      BODY
         *      RETURNS
         *      {
         *          [
         *              {
         *                  "id": "sffgjlkdgjhsdsfdlkfjghsdfgsdlkjfgh",
         *                  "name": "I am a library name"
         *              },
         *              ...
         *          ]
         *      }
         *
         * </remarks>
         * <response code="200">Returns a list of vsphere libraries.</response>
         * <response code="550">Error code from vsphere.</response>
         */
        [HttpGet("libraries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(550)]
        public async Task<ActionResult<IEnumerable<ContentLibrary>>> GetLibraries()
        {
            try
            {
                return Ok(_vCenter.GetContentLibraries());
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }


        /**
         * <summary>
         * Gets a list of folders from vsphere
         * </summary>
         * <returns>A list of vsphere folders, where each list member contains the vsphere name and identifier of the folder.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/CreateVm/folders
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      GET /api/CreateVm/folders
         *      BODY
         *      RETURNS
         *      {
         *          [
         *              {
         *                  "name": "I am a folder name",
         *                  "folder": "folder3526"
         *              },
         *              ...
         *          ]
         *      }
         *
         * </remarks>
         * <response code="200">Returns a list of vsphere folders.</response>
         * <response code="550">Error code from vsphere.</response>
         */
        [HttpGet("folders")]
        public async Task<ActionResult<IEnumerable<OldFolder>>> GetFolders()
        {
            try
            {
                return Ok(_vCenter.GetFolders());
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }


        /**
         * <summary>
         * Gets a list of templates from vsphere given a vsphere library id
         * </summary>
         * <returns>A list of vsphere folders, where each list member contains the vsphere name and identifier of the folder.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/CreateVm/templates/{id}
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      GET /api/CreateVm/templates/asdjhkgjjshdgfkasd
         *      BODY
         *      RETURNS
         *      {
         *          [
         *              {
         *                  "id": "asdfghjkldfghjkl",
         *                  "name": "I am a vsphere template name"
         *              },
         *              ...
         *          ]
         *      }
         *
         * </remarks>
         * <response code="200">Returns a list of vsphere templates from a library.</response>
         * <response code="550">Error code from vsphere.</response>
         */
        [HttpGet("templates/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetTemplates(string Id)
        {
            try
            {
                //check if it is a professor
                User professorUser = _authorization.GetAuth("admin");

                if (professorUser is not null)
                {
                    return Ok(_vCenter.GetTemplatesByContentLibraryId(Id));
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