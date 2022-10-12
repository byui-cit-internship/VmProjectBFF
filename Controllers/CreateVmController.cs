using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using vmProjectBFF.DTO;
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
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            //Open uri communication
            var httpClient = HttpClientFactory.CreateClient();
            // Basic authentication in base64
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11xMQ==";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.cit.byui.edu/api/session", null);
            if (tokenResponse.IsSuccessStatusCode)
            {
                string tokenstring = " ";
                //Turn this object into a readable string
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();

                //Scape characters functions to filter the new header results
                tokenstring = tokenstring.Replace("\"", "");
                tokenstring = tokenstring.Replace("{", "");
                tokenstring = tokenstring.Replace("value:", "");
                tokenstring = tokenstring.Replace("}", "");
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                //Make a list of Library objects
                List<Library> libraries = new List<Library>();
                //Call library uri in vsphere
                var responseLibraryIds = await httpClient.GetAsync("https://vctr-dev.cit.byui.edu/api/content/local-library");
                //Turn these objects responses into a readable string
                string responseStringLibraries = await responseLibraryIds.Content.ReadAsStringAsync();
                //Make a list of library id's   
                List<String> libraryIds = JsonConvert.DeserializeObject<List<String>>(responseStringLibraries);
                //Create a list using our Dto          
                foreach (string libraryId in libraryIds)
                {
                    // return Ok(libraryIds);
                    var libraryresponse = await httpClient.GetAsync($"https://vctr-dev.cit.byui.edu/api/content/local-library/" + libraryId);
                    string response2String = await libraryresponse.Content.ReadAsStringAsync();
                    Library library = JsonConvert.DeserializeObject<Library>(response2String);
                    libraries.Add(library);
                }
                var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.cit.byui.edu/api/session");
                return Ok(libraries);
                // if (libraries != null)
                // {

                //     httpClient.DefaultRequestHeaders.Add("Authorization", base64);
                //     httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

                //     var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.cit.byui.edu/rest/com/vmware/cis/session");
                //     return Ok("Session was deleted here");
                // }
            }
            return StatusCode(550, "Bad response from vsphere.");
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
            //Open uri communication
            var httpClient = HttpClientFactory.CreateClient();
            // Basic authentication in base64
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11xMQ==";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.cit.byui.edu/api/session", null);
            if (tokenResponse.IsSuccessStatusCode)
            {
                string tokenstring = " ";
                //Turn this object into a readable string
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();

                //Scape characters functions to filter the new header results
                tokenstring = tokenstring.Replace("\"", "");
                tokenstring = tokenstring.Replace("{", "");
                tokenstring = tokenstring.Replace("value:", "");
                tokenstring = tokenstring.Replace("}", "");
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                //Make a list of Library object
                //Call library uri in vsphere
                var responseFolders = await httpClient.GetAsync("https://vctr-dev.cit.byui.edu/rest/vcenter/folder?filter.type=VIRTUAL_MACHINE");
                //Turn these objects responses into a readable string
                //
                string folderResponseString = await responseFolders.Content.ReadAsStringAsync();


                FolderResponse folderResponse = JsonConvert.DeserializeObject<FolderResponse>(folderResponseString);
                List<OldFolder> folders = new List<OldFolder>();

                //declare variable from configuration (appsettings.json)
                string ignoreFolder = _configuration["IgnoreFolder"];

                var deleteResponse1 = await httpClient.DeleteAsync("https://vctr-dev.cit.byui.edu/api/session");

                foreach (OldFolder folder in folderResponse.value)
                {
                    if (folder.name != ignoreFolder)
                        // string response2String = await response2.Content.ReadAsStringAsync();
                        // Folder folder = JsonConvert.DeserializeObject<Folder>(response2String);
                        folders.Add(folder);
                }

                return Ok(folders);
            }
            var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.cit.byui.edu/api/session");
            return StatusCode(550);
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
            string userEmail = HttpContext.User.Identity.Name;
            //check if it is a professor
            User professorUser = _authorization.GetAuth("admin");

            if (professorUser != null)
            {
                // Creating the client request and setting headers to the request
                var httpClient = HttpClientFactory.CreateClient();
                string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
                //Adding headers
                httpClient.DefaultRequestHeaders.Add("Authorization", base64);
                var tokenResponse = await httpClient.PostAsync("https://vctr-dev.cit.byui.edu/rest/com/vmware/cis/session", null);
                string tokenstring = " ";
                //Turn this object into a readable string
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                //Scape characters functions to filter the new header results
                tokenstring = tokenstring.Replace("\"", "");
                tokenstring = tokenstring.Replace("{", "");
                tokenstring = tokenstring.Replace("value:", "");
                tokenstring = tokenstring.Replace("}", "");
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                List<Template> templates = new List<Template>();
                // 
                var response = await httpClient.GetAsync($"https://vctr-dev.cit.byui.edu/api/content/library/item?library_id={Id}");
                string responseString = await response.Content.ReadAsStringAsync();
                // return Ok(responseString);
                List<String> templateIds = templateIds = JsonConvert.DeserializeObject<List<String>>(responseString);
                foreach (string templateId in templateIds)
                {
                    var response2 = await httpClient.GetAsync($"https://vctr-dev.cit.byui.edu/api/content/library/item/" + templateId);
                    Console.WriteLine($"Second response {response2}");
                    string response2String = await response2.Content.ReadAsStringAsync();
                    Template template = JsonConvert.DeserializeObject<Template>(response2String);
                    templates.Add(template);
                }
                var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.cit.byui.edu/api/session");
                return Ok(templates);
            }
            return StatusCode(550);
        }
    }
}