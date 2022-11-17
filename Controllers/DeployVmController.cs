﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VmProjectBFF.DTO;
using VmProjectBFF.DTO.Database;
using VmProjectBFF.DTO.VCenter;
using VmProjectBFF.Exceptions;
using VmProjectBFF.Services;

namespace VmProjectBFF.Controllers
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
        public async Task<ActionResult> PostVmTable([FromBody] int enrollment_id)
        {
            try
            {
                User studentUser = _authorization.GetAuth("user");

                if (studentUser is not null)
                {
                    // "[{\"student_name\":\"Trevor Wayman\",\"course_name\":\"CIT 270\",\"course_id\":1,\"template_id\":\"cit270-empty-vm-template\",\"course_semester\":\"Spring\",\"enrollment_id\":33,\"folder\":\"CIT270\"}]"
                    CreateVmDTO createVm = _backend.GetCreateVmByEnrollmentId(enrollment_id); // Should validation be added so createVm is not made by any student on behalf of another student??

                    // Create vm with the information we have in vsphere
                    DeployContainer deployContainer = new()
                    {
                    spec = new Deploy  
                    {
                        name = HttpContext.User.Identity.Name,
                        placement = new Placement
                        {
                            folder = createVm.Folder, // Check CIT270 error 
                            resource_pool = "resgroup-2014"
                        }
                    }
                    };

                    NewVmInstance vCenterInstanceId = _vCenter.NewVmInstanceByTemplateId(createVm.Template_id,
                                                                                  deployContainer);

                    DTO.Database.VmTemplate template = _backend.GetTemplateByVCenterId(createVm.Template_id);

                    VmInstance vmInstance = new()
                    {
                        VmInstanceVcenterId = vCenterInstanceId.value,
                        VmTemplateId = template.VmTemplateId,
                        VmInstanceExpireDate = DateTime.MaxValue
                    };

                    return Ok(_backend.CreateVmInstance(vmInstance));
                }
                return Forbid();
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }

        [HttpGet("resource-pool")]
        public async Task<ActionResult<IEnumerable<Pool>>> GetPools()
        {
            try
            {
                return Ok(_vCenter.GetResourceGroups());
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}
