﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VmProjectBFF.DTO;
using VmProjectBFF.DTO.Database;
using DBFolder = VmProjectBFF.DTO.Database.Folder;
using VmProjectBFF.DTO.VCenter;
using VCPool = VmProjectBFF.DTO.VCenter.Pool;
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
        public async Task<ActionResult> PostVmTable([FromBody] DeployVmRequirements requirements) //int enrollment_id, string vmInstanceName
        {
            try
            {
                User studentUser = _authorization.GetAuth("user");

                if (studentUser is not null)
                {
                    CreateVmDTO createVm = _backend.GetCreateVmByEnrollmentId(requirements.enrollmentId); // Should validation be added so createVm is not made by any student on behalf of another student??
                    Section section = _backend.GetSectionsByEnrollmentId(requirements.enrollmentId);
                    Semester semester = _backend.GetSemesterBySemesterId(section.SemesterId);
                    ResourcePool resourcePool = _backend.GetResourcePoolByResourcePoolId(section.ResourcePoolId);
                    DBFolder folder = _backend.GetFolderByFolderId(section.FolderId);

                    // Create vm with the information we have in vsphere
                    DeployContainer deployContainer = new()
                    {
                    spec = new Deploy
                    {
                        name = requirements.vmInstanceName,
                        placement = new Placement
                        {
                            folder = folder.VcenterFolderId, // Check CIT270 error 
                            resource_pool = resourcePool.ResourcePoolVsphereId
                        }
                    }
                    };

                    NewVmInstance vCenterInstanceId = _vCenter.NewVmInstanceByTemplateId(requirements.templateId,deployContainer);
                                    
                    _vCenter.StartVm(vCenterInstanceId.value);

                    VmInstance vmInstance = new()
                    {
                        VmInstanceVcenterId = vCenterInstanceId.value,
                        VmTemplateId = requirements.templateId,
                        VmInstanceCreationDate = requirements.vmInstanceCreationDate,
                        VmInstanceExpireDate = semester.EndDate,
                        VmInstanceVcenterName = requirements.vmInstanceName,
                        SectionId = section.SectionId,
                        /* added this to replace tagUser*/
                        UserId = studentUser.UserId
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
        public async Task<ActionResult<IEnumerable<VCPool>>> GetPools()
        {
            try
            {
                return Ok(_vCenter.GetResourcePools());
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }

        [HttpGet("vm-network")]
        public async Task<ActionResult<IEnumerable<VmNetworkInfoContainer>>> GetVmNetworkInfo([FromQuery] string vmInstanceVcenterId)
        {
            try
            {
                return Ok(_vCenter.GetVmNetworkInfo(vmInstanceVcenterId));
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}
