using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vmProjectBFF.Models;
using vmProjectBFF.DTO;
using System.Net.Http;
using Newtonsoft.Json;
using vmProjectBFF.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace vmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VmTableController : ControllerBase

    {

        private readonly Authorization _auth;
        private readonly Backend _backend;
        private readonly IConfiguration _configuration;
        private readonly ILogger<VmTableController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VmTableController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<VmTableController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _auth = new(_backend, _logger);
            _httpClientFactory = httpClientFactory;
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

                    // Creating the client request and setting headers
                    var httpClient = _httpClientFactory.CreateClient();
                    string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
                    Console.WriteLine(base64);

                    httpClient.DefaultRequestHeaders.Add("Authorization", base64);

                    var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/api/session", null);
                    Console.WriteLine(tokenResponse);
                    string tokenstring = " ";
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                        //Taking quotes out of the tokenstring variable s = s.Replace("\"", "");
                        tokenstring = tokenstring.Replace("\"", "");

                        Console.WriteLine($"it was sucessfull {tokenstring}");


                    }
                    httpClient.DefaultRequestHeaders.Remove("Authorization");
                    //we are removing the basic auth because it require a new authorization
                    httpClient.DefaultRequestHeaders.Add("vmware-api-session-id", tokenstring);
                    // contains our base Url where templates were added in vcenter
                    // This URL enpoint gives a list of all the Templates we have in our vcenter 
                    List<Template> templates = new List<Template>();

                    var response = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item?library_id={libraryId}");
                    Console.WriteLine($" response to the second call {response}");

                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("template response" + responseString);
                    List<String> templateIds = templateIds = JsonConvert.DeserializeObject<List<String>>(responseString);


                    //call Api, convert it to templates, and get the list of templates
                    foreach (string templateId in templateIds)
                    {
                        var response2 = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item/" + templateId);
                        Console.WriteLine($"Second response {response2}");


                        string response2String = await response2.Content.ReadAsStringAsync();
                        Template template = JsonConvert.DeserializeObject<Template>(response2String);
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