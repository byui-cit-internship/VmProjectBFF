using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VmProjectBFF.DTO;
using VmProjectBFF.DTO.Database;
using VmProjectBFF.Exceptions;
using VmProjectBFF.Services;

namespace VmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : BffController
    {

        public EnrollmentController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<EnrollmentController> logger,
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


        /****************************************
        Allows professor to create a course in the database using a canvas course id, a course name,
        a course code, a canavs api token, a section number, a semester, and vm template id
        ****************************************/
        [HttpPost("professor/register/course")]
        public async Task<ActionResult<CourseCreate>> CreateCourseEnrollment([FromBody] CourseCreate courseDetails)
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor != null && courseDetails != null)
                {
                    _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "sectionCanvasId", courseDetails.canvasCourseId } });
                    Section courseExist = JsonConvert.DeserializeObject<Section>(_lastResponse.Response);

                    if (courseExist == null)
                    {
                        _lastResponse = _backendHttpClient.Get($"api/v2/Course", new() { { "courseId", courseDetails.course_id } });
                        Course course = JsonConvert.DeserializeObject<Course>(_lastResponse.Response);

                        if (course == null)
                        {

                            _lastResponse = _backendHttpClient.Get($"api/v2/ResourceGroup", new() { { "memory", 0 }, { "cpu", 0 }, { "resourceGroupName", courseDetails.resource_group } });
                            ResourceGroup resourceGroupCourse = JsonConvert.DeserializeObject<ResourceGroup>(_lastResponse.Response);
                            if (resourceGroupCourse == null)
                            {
                                resourceGroupCourse = new();
                                resourceGroupCourse.Cpu = 0;
                                resourceGroupCourse.Memory = 0;
                                resourceGroupCourse.ResourceGroupName = courseDetails.resource_group;
                                _lastResponse = _backendHttpClient.Post("api/v2/ResourceGroup", resourceGroupCourse);
                                resourceGroupCourse = JsonConvert.DeserializeObject<ResourceGroup>(_lastResponse.Response);
                            }

                            course = new();
                            course.CourseCode = courseDetails.courseCode;
                            course.ResourceGroupId = resourceGroupCourse.ResourceGroupId;

                            _lastResponse = _backendHttpClient.Post("api/v2/Course", course);
                            course = JsonConvert.DeserializeObject<Course>(_lastResponse.Response);
                        }

                        _lastResponse = _backendHttpClient.Get($"api/v2/Folder", new() { { "vcenterFolderId", courseDetails.folder } });
                        Folder folder = JsonConvert.DeserializeObject<Folder>(_lastResponse.Response);

                        if (folder == null)
                        {
                            folder = new();
                            folder.VcenterFolderId = courseDetails.folder;
                            _lastResponse = _backendHttpClient.Post($"api/v2/Folder", folder);
                            folder = JsonConvert.DeserializeObject<Folder>(_lastResponse.Response);
                        }

                        professor.CanvasToken = courseDetails.canvas_token;
                        _lastResponse = _backendHttpClient.Put("api/v2/User", professor);
                        professor = JsonConvert.DeserializeObject<User>(_lastResponse.Response);


                        _lastResponse = _backendHttpClient.Get($"api/v2/Semester", new() { { "enrollmentTermId", courseDetails.semester.EnrollmentTermId } });
                        Semester term = JsonConvert.DeserializeObject<Semester>(_lastResponse.Response);

                        if (term == null)
                        {
                            _lastResponse = _backendHttpClient.Post($"api/v2/Semester", courseDetails.semester);
                            courseDetails.semester = JsonConvert.DeserializeObject<Semester>(_lastResponse.Response);
                            term = courseDetails.semester;
                        }

                        _lastResponse = _backendHttpClient.Get($"api/v2/ResourceGroup", new() { { "resourceGroupId", course.ResourceGroupId } });
                        ResourceGroup resourceGroupTemplate = JsonConvert.DeserializeObject<ResourceGroup>(_lastResponse.Response);

                        ResourceGroup resourceGroupSection = new();
                        resourceGroupSection.Cpu = resourceGroupTemplate.Cpu;
                        resourceGroupSection.Memory = resourceGroupTemplate.Memory;
                        resourceGroupSection.ResourceGroupName = $"{resourceGroupTemplate.ResourceGroupName}-{courseDetails.resource_group}";

                        _lastResponse = _backendHttpClient.Post($"api/v2/ResourceGroup", resourceGroupSection);
                        resourceGroupSection = JsonConvert.DeserializeObject<ResourceGroup>(_lastResponse.Response);

                        List<DTO.Database.VmTemplate> vmTemplates = new();
                        foreach (string templateVmId in courseDetails.templateVm)
                        {
                            _lastResponse = _backendHttpClient.Get($"api/v2/VmTemplate", new() { { "vmTemplateVcenterId", templateVmId } });
                            DTO.Database.VmTemplate template = JsonConvert.DeserializeObject<DTO.Database.VmTemplate>(_lastResponse.Response);

                            if (template == null)
                            {
                                template = new VmTemplate();
                                template.VmTemplateVCenterId = templateVmId;
                                template.VmTemplateVCenterName = "test";
                                template.VmTemplateAccessDate = new DateTime(2022, 1, 1);
                                template.LibraryVCenterId = courseDetails.libraryId;

                                _lastResponse = _backendHttpClient.Post($"api/v2/VmTemplate", template);
                                template = JsonConvert.DeserializeObject<DTO.Database.VmTemplate>(_lastResponse.Response);
                            }

                            vmTemplates.Add(template);
                        }

                        Section newSection = new();
                        newSection.CourseId = (int)course.CourseId;
                        newSection.SectionCanvasId = Int32.Parse(courseDetails.canvasCourseId);
                        newSection.SemesterId = term.SemesterId;
                        newSection.SectionNumber = courseDetails.section_num;
                        newSection.FolderId = folder.FolderId;
                        newSection.ResourceGroupId = resourceGroupSection.ResourceGroupId;
                        newSection.SectionName = courseDetails.sectionName;
                        newSection.LibraryVCenterId = courseDetails.libraryId;

                        _lastResponse = _backendHttpClient.Post($"api/v2/Section", newSection);
                        newSection = JsonConvert.DeserializeObject<Section>(_lastResponse.Response);

                        _lastResponse = _backendHttpClient.Get($"api/v2/Role", new() { { "roleName", "Professor" } });
                        Role profRole = JsonConvert.DeserializeObject<List<Role>>(_lastResponse.Response).FirstOrDefault();

                        if (profRole == null)
                        {
                            profRole = new();
                            profRole.RoleName = "Professor";
                            profRole.CanvasRoleId = 56898;

                            _lastResponse = _backendHttpClient.Post($"api/v2/Role", profRole);
                            profRole = JsonConvert.DeserializeObject<Role>(_lastResponse.Response);
                        }

                        _lastResponse = _backendHttpClient.Get($"api/v2/TagCategory", new() { { "tagCategoryName", "Course" } });
                        TagCategory tagCategory = JsonConvert.DeserializeObject<TagCategory>(_lastResponse.Response);

                        if (tagCategory == null)
                        {
                            tagCategory = new();
                            tagCategory.TagCategoryVcenterId = "TEMP_USE";
                            tagCategory.TagCategoryName = "Course";
                            tagCategory.TagCategoryDescription = "Do not attempt to use with VCenter.";

                            _lastResponse = _backendHttpClient.Post($"api/v2/TagCategory", tagCategory);
                            tagCategory = JsonConvert.DeserializeObject<TagCategory>(_lastResponse.Response);
                        }

                        _lastResponse = _backendHttpClient.Get($"api/v2/Tag", new() { { "tagCategoryId", tagCategory.TagCategoryId }, { "tagName", course.CourseCode } });
                        Tag tag = JsonConvert.DeserializeObject<Tag>(_lastResponse.Response);

                        if (tag == null)
                        {
                            tag = new();
                            tag.TagVCenterId = "TEMP_USE";
                            tag.TagCategoryId = tagCategory.TagCategoryId;
                            tag.TagName = course.CourseCode;
                            tag.TagDescription = "Do not attempt to use with VCenter.";

                            _lastResponse = _backendHttpClient.Post($"api/v2/Tag", tag);
                            tag = JsonConvert.DeserializeObject<Tag>(_lastResponse.Response);
                        }

                        foreach (DTO.Database.VmTemplate vmTemplate in vmTemplates)
                        {
                            _lastResponse = _backendHttpClient.Get($"api/v2/VmTemplateTag", new() { { "tagId", tag.TagId }, { "vmTemplateId", vmTemplate.VmTemplateId } });
                            VmTemplateTag vmTemplateTag = JsonConvert.DeserializeObject<VmTemplateTag>(_lastResponse.Response);

                            if (vmTemplateTag == null)
                            {
                                vmTemplateTag = new();
                                vmTemplateTag.VmTemplateId = vmTemplate.VmTemplateId;
                                vmTemplateTag.TagId = tag.TagId;

                                _lastResponse = _backendHttpClient.Post($"api/v2/VmTemplateTag", vmTemplateTag);
                                vmTemplateTag = JsonConvert.DeserializeObject<VmTemplateTag>(_lastResponse.Response);
                            }
                        }

                        UserSectionRole enrollment = new UserSectionRole
                        {
                            UserId = professor.UserId,
                            RoleId = profRole.RoleId,
                            SectionId = (int)newSection.SectionId
                        };

                        _lastResponse = _backendHttpClient.Post($"api/v2/UserSectionRole", enrollment);
                        enrollment = JsonConvert.DeserializeObject<UserSectionRole>(_lastResponse.Response);

                        return Ok("ID " + newSection.SectionId + " enrollment was created");
                    }
                    else
                    {
                        return Conflict(new { message = $"A course already exits with this id {courseDetails.course_id}" });
                    }
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
