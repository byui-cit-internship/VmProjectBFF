using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;
using vmProjectBFF.Models;
using vmProjectBFF.Services;

namespace vmProjectBFF.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeployVmController : BffController
    {

        public DeployVmController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<DeployVmController> logger,
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

        //Connect our API to a second API that creates our vms 
        [HttpPost()]
        public async Task<ActionResult> PostVmTable([FromBody] string enrollment_id)
        {
            try
            {
                User studentUser = _authorization.GetAuth("user");

                if (studentUser != null)
                {
                    // "[{\"student_name\":\"Trevor Wayman\",\"course_name\":\"CIT 270\",\"course_id\":1,\"template_id\":\"cit270-empty-vm-template\",\"course_semester\":\"Spring\",\"enrollment_id\":33,\"folder\":\"CIT270\"}]"
                    _lastResponse = _backendHttpClient.Get("api/v1/CreateVm", new() { { "enrollmentId", enrollment_id } });
                    CreateVmDTO createVm = JsonConvert.DeserializeObject<List<CreateVmDTO>>(_lastResponse.Response).FirstOrDefault(); // Should validation be added so createVm is not made by any student on behalf of another student??

                    string template_id = createVm.Template_id;

                    // Create a session token
                    var httpClient = HttpClientFactory.CreateClient();
                    string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11xMQ==";
                    //Adding headers
                    httpClient.DefaultRequestHeaders.Add("Authorization", base64);
                    var tokenResponse = await httpClient.PostAsync("https://vctr-dev.cit.byui.edu/api/session", null);
                    Console.WriteLine(tokenResponse);
                    string tokenstring = " ";
                    tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                    //Taking quotes out of the tokenstring variable s = s.Replace("\"", "");
                    tokenstring = tokenstring.Replace("\"", "");
                    tokenstring = tokenstring.Replace("{", "");
                    tokenstring = tokenstring.Replace("value:", "");
                    tokenstring = tokenstring.Replace("}", "");
                    httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        string resourcePool = _configuration["Resource_Pool"];
                        // Create vm with the information we have in vsphere
                        var deploy = new Deploy
                        {

                            name = HttpContext.User.Identity.Name,
                            placement = new Placement
                            {
                                folder = createVm.Folder, // Check CIT270 error 

                                resource_pool = resourcePool

                            }
                        };
                        var deployResult = JsonConvert.SerializeObject(deploy);

                        // var content = new StringContent(deployResult);

                        var deployContent = new StringContent(deployResult, Encoding.UTF8, "application/json");

                        // var content2 = new StringContent(content, Encoding.UTF8, "application/json");

                        // return Ok(content2);

                        var postResponse = await httpClient.PostAsync($"https://vctr-dev.cit.byui.edu/api/vcenter/vm-template/library-items/{template_id}?action=deploy", deployContent); // Here I get a 404

                        var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.cit.byui.edu/api/session");
                        //  var content2 = await postResponse.Content.ReadAsStringAsync();

                        //  var createdCompany = JsonSerializer.Deserialize<DeployDto>(content, _options);
                        _lastResponse = _backendHttpClient.Get("api/v2/VmTemplate", new() { { "vmTemplateVcenterId", template_id } });
                        VmTemplate template = JsonConvert.DeserializeObject<VmTemplate>(_lastResponse.Response);

                        VmInstance vmInstance = new();
                        vmInstance.VmInstanceVcenterId = postResponse.Content.ReadAsStringAsync().Result;
                        vmInstance.VmTemplateId = template.VmTemplateId;
                        vmInstance.VmInstanceExpireDate = DateTime.MaxValue;

                        _lastResponse = _backendHttpClient.Post("api/v1/CreateVm", vmInstance);
                        vmInstance = JsonConvert.DeserializeObject<VmInstance>(_lastResponse.Response);

                        return Ok(vmInstance);
                    }
                    return Ok("here session");
                }
                return Unauthorized("You are not Authorized and this is not a student");
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }

        [HttpGet("resource-pool")]
        public async Task<ActionResult<IEnumerable<Pool>>> GetPools()
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
                //Call resource-pool uri in vsphere
                var response = await httpClient.GetAsync("https://vctr-dev.cit.byui.edu/api/vcenter/resource-pool");
                //Turn these objects responses into a readable string
                string poolResponseString = await response.Content.ReadAsStringAsync();
                // Pool poolResponse = JsonConvert.DeserializeObject<Pool>(poolResponseString);
                var objResponse = JsonConvert.DeserializeObject<List<Pool>>(poolResponseString);
                return Ok(objResponse);
            }
            return Unauthorized("here");

        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteSession()
        {
            // Create a session token
            var httpClient = HttpClientFactory.CreateClient();
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11xMQ==";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.cit.byui.edu/api/session", null);
            string tokenstring = " ";
            tokenstring = await tokenResponse.Content.ReadAsStringAsync();
            //Scape characters functions to filter the new header results
            tokenstring = tokenstring.Replace("\"", "");
            tokenstring = tokenstring.Replace("{", "");
            tokenstring = tokenstring.Replace("value:", "");
            tokenstring = tokenstring.Replace("}", "");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

            var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.cit.byui.edu/api/session");

            return Ok("Session Deleted");
        }
    }
}
