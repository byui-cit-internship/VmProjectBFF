using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using vmProjectBFF.DTO;
using vmProjectBFF.Models;

namespace vmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VmTableController : BffController
    {

        public VmTableController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<VmTableController> logger)
            : base(
                  configuration: configuration,
                  httpClientFactory: httpClientFactory,
                  httpContextAccessor: httpContextAccessor,
                  logger: logger)
        {
        }

        //GET: api/vmtable/templates
        [HttpGet("templates/all")]
        public async Task<ActionResult<IEnumerable<string>>> GetTemplates(string libraryId)
        {
            User professorUser = _auth.getAuth("admin");

            if (professorUser != null)
            {
                try
                {
                    // contains our base Url where templates were added in vcenter
                    // This URL enpoint gives a list of all the Templates we have in our vcenter 
                    List<Template> templates = new List<Template>();

                    _lastResponse = _vCenter.Get($"api/content/library/item?library_id={libraryId}");
                    List<string> templateIds = templateIds = JsonConvert.DeserializeObject<List<String>>(_lastResponse.Response);


                    //call Api, convert it to templates, and get the list of templates
                    foreach (string templateId in templateIds)
                    {
                        _lastResponse = _vCenter.Get($"api/content/library/item/{templateId}");
                        Template template = JsonConvert.DeserializeObject<Template>(_lastResponse.Response);
                        templates.Add(template);
                    }
                    if (templates != null)
                    {
                        return Ok(templates);
                    }
                    else
                    {
                        return NotFound("Failed calling");
                    }
                }
                catch
                {
                    return Problem("crash returning templates!");

                }

            }
            return Unauthorized("You are not Authorized and is not a professor");

        }
    }
}