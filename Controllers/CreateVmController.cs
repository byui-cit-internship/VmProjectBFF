using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VmProjectBFF.DTO.Database;
using VmProjectBFF.DTO.VCenter;
using VmProjectBFF.Exceptions;
using VmProjectBFF.Services;
using VCFolder = VmProjectBFF.DTO.VCenter.Folder;
using Newtonsoft.Json;

namespace VmProjectBFF.Controllers
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

        [HttpGet("libraryById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(550)]
        public async Task<ActionResult<IEnumerable<ContentLibrary>>> GetLibrariesById(string id)
        {
            try
            {
                return Ok(_vCenter.GetContentLibraryById(id));
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
        public async Task<ActionResult<IEnumerable<VCFolder>>> GetFolders()
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

        [HttpGet("vCenterTemplate/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetVCenterTemplateById(string id)
        {
            try
            {
                User professorUser = _authorization.GetAuth("admin");

                if (professorUser is not null)
                {
                    return Ok(_vCenter.GetTemplateByVCenterId(id));
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

        [HttpGet("vCenterTemplate/metadata/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetVCenterTemplateMetadata(string id)
        {
            try
            {
                User professorUser = _authorization.GetAuth("admin");

                if (professorUser is not null)
                {
                    return Ok(_vCenter.GetTemplateMetadata(id));
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



        [HttpPost("templates/postTemplate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> PostTemplate([FromBody] DTO.Database.VmTemplate template)
        {
            try
            {
                User professor = _authorization.GetAuth("admin");
                if (professor is not null)
                {
                    _lastResponse = _backendHttpClient.Get($"api/v2/VmTemplate", new() { { "vmTemplateVcenterId", template.VmTemplateId } });
                    DTO.Database.VmTemplate fetchedTemplate = JsonConvert.DeserializeObject<DTO.Database.VmTemplate>(_lastResponse.Response);

                    if (fetchedTemplate is not null)
                    {
                        _lastResponse = _backendHttpClient.Post($"api/v2/VmTemplate", template);
                        template = JsonConvert.DeserializeObject<DTO.Database.VmTemplate>(_lastResponse.Response);
                        return Ok(template);
                    }
                    return Conflict();
                }
                return Unauthorized();
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}