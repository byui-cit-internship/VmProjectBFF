using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.Models;
using vmProjectBackend.DTO;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using vmProjectBackend.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace vmProjectBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreateVmController : ControllerBase
    {
        private readonly Authorization _auth;
        private readonly Backend _backend;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateVmController> _logger;

        public CreateVmController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CreateVmController> logger)
        {
            _logger = logger; _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _auth = new(_backend, _logger);
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("libraries")]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            //Open uri communication
            var httpClient = _httpClientFactory.CreateClient();
            // Basic authentication in base64
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
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
                var responseLibraryIds = await httpClient.GetAsync("https://vctr-dev.citwdd.net/api/content/local-library");
                //Turn these objects responses into a readable string
                string responseStringLibraries = await responseLibraryIds.Content.ReadAsStringAsync();
                //Make a list of library id's   
                List<String> libraryIds = JsonConvert.DeserializeObject<List<String>>(responseStringLibraries);
                //Create a list using our Dto          
                foreach (string libraryId in libraryIds)
                {
                    // return Ok(libraryIds);
                    var libraryresponse = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/local-library/" + libraryId);
                    string response2String = await libraryresponse.Content.ReadAsStringAsync();
                    Library library = JsonConvert.DeserializeObject<Library>(response2String);
                    libraries.Add(library);
                }
                return Ok(libraries);
                // if (libraries != null)
                // {

                //     httpClient.DefaultRequestHeaders.Add("Authorization", base64);
                //     httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

                //     var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");
                //     return Ok("Session was deleted here");
                // }
            }
            return Unauthorized("here");
        }

        [HttpGet("folders")]
        public async Task<ActionResult<IEnumerable<OldFolder>>> GetFolders()
        {
            //Open uri communication
            var httpClient = _httpClientFactory.CreateClient();
            // Basic authentication in base64
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
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
                var responseFolders = await httpClient.GetAsync("https://vctr-dev.citwdd.net/rest/vcenter/folder?filter.type=VIRTUAL_MACHINE");
                //Turn these objects responses into a readable string
                //
                string folderResponseString = await responseFolders.Content.ReadAsStringAsync();


                FolderResponse folderResponse = JsonConvert.DeserializeObject<FolderResponse>(folderResponseString);
                List<OldFolder> folders = new List<OldFolder>();

                //declare variable from configuration (appsettings.json)
                string ignoreFolder = _configuration["IgnoreFolder"];


                foreach (OldFolder folder in folderResponse.value)
                {
                    if (folder.name != ignoreFolder)
                        // string response2String = await response2.Content.ReadAsStringAsync();
                        // Folder folder = JsonConvert.DeserializeObject<Folder>(response2String);
                        folders.Add(folder);
                }

                return Ok(folders);
            }
            return Unauthorized("here");
        }

        [HttpGet("templates/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetTemplates(string Id)
        {
            string userEmail = HttpContext.User.Identity.Name;
            //check if it is a professor
            User professorUser = _auth.getAuth("admin");

            if (professorUser != null)
            {
                // Creating the client request and setting headers to the request
                var httpClient = _httpClientFactory.CreateClient();
                string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
                //Adding headers
                httpClient.DefaultRequestHeaders.Add("Authorization", base64);
                var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
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
                var response = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item?library_id={Id}");
                string responseString = await response.Content.ReadAsStringAsync();
                // return Ok(responseString);
                List<String> templateIds = templateIds = JsonConvert.DeserializeObject<List<String>>(responseString);
                foreach (string templateId in templateIds)
                {
                    var response2 = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item/" + templateId);
                    Console.WriteLine($"Second response {response2}");
                    string response2String = await response2.Content.ReadAsStringAsync();
                    Template template = JsonConvert.DeserializeObject<Template>(response2String);
                    templates.Add(template);
                }
                return Ok(templates);
            }
            return Unauthorized();
        }
    }
}