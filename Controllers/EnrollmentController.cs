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
            IBackgroundService1Manual backgroundService1,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<EnrollmentController> logger,
            IVCenterRepository vCenter)
            : base(
                  authorization: authorization,
                  backend: backend,
                  backgroundService1: backgroundService1,
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
                        _lastResponse = _backendHttpClient.Get($"api/v2/Course", new() { { "courseCode", courseDetails.courseCode } });
                        Course course = JsonConvert.DeserializeObject<Course>(_lastResponse.Response);

                        if (course == null)
                        {

                            _lastResponse = _backendHttpClient.Get($"api/v2/ResourcePool", new() { { "memory", 0 }, { "cpu", 0 }, { "resourcePoolName", courseDetails.resourcePoolName } });
                            ResourcePool resourcePoolCourse = JsonConvert.DeserializeObject<ResourcePool>(_lastResponse.Response);
                            if (resourcePoolCourse == null)
                            {
                                resourcePoolCourse = new();
                                resourcePoolCourse.Cpu = 0;
                                resourcePoolCourse.Memory = 0;
                                resourcePoolCourse.ResourcePoolName = courseDetails.resourcePoolName;
                                resourcePoolCourse.ResourcePoolVsphereId = courseDetails.resource_pool;
                                _lastResponse = _backendHttpClient.Post("api/v2/ResourcePool", resourcePoolCourse);
                                resourcePoolCourse = JsonConvert.DeserializeObject<ResourcePool>(_lastResponse.Response);
                            }

                            course = new();
                            course.CourseCode = courseDetails.courseCode;
                            course.ResourcePoolId = resourcePoolCourse.ResourcePoolId;

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


                        _lastResponse = _backendHttpClient.Get($"api/v2/Semester", new() { { "enrollmentTermCanvasId", courseDetails.semester.EnrollmentTermCanvasId } });
                        Semester term = JsonConvert.DeserializeObject<Semester>(_lastResponse.Response);

                        if (term == null)
                        {
                            _lastResponse = _backendHttpClient.Post($"api/v2/Semester", courseDetails.semester);
                            courseDetails.semester = JsonConvert.DeserializeObject<Semester>(_lastResponse.Response);
                            term = courseDetails.semester;
                        }

                        _lastResponse = _backendHttpClient.Get($"api/v2/ResourcePool", new() { { "resourcePoolId", course.ResourcePoolId } });
                        ResourcePool resourcePoolTemplate = JsonConvert.DeserializeObject<ResourcePool>(_lastResponse.Response);

                        ResourcePool resourcePoolSection = new();
                        resourcePoolSection.Cpu = resourcePoolTemplate.Cpu;
                        resourcePoolSection.Memory = resourcePoolTemplate.Memory;
                        resourcePoolSection.ResourcePoolName = resourcePoolTemplate.ResourcePoolName;
                        resourcePoolSection.ResourcePoolVsphereId = resourcePoolTemplate.ResourcePoolVsphereId;

                        _lastResponse = _backendHttpClient.Post($"api/v2/ResourcePool", resourcePoolSection);
                        resourcePoolSection = JsonConvert.DeserializeObject<ResourcePool>(_lastResponse.Response);

                        Section newSection = new();
                        newSection.CourseId = (int)course.CourseId;
                        newSection.SectionCanvasId = Int32.Parse(courseDetails.canvasCourseId);
                        newSection.SemesterId = term.SemesterId;
                        newSection.SectionNumber = courseDetails.section_num;
                        newSection.FolderId = folder.FolderId;
                        newSection.ResourcePoolId = resourcePoolSection.ResourcePoolId;
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

                        UserSectionRole enrollment = new UserSectionRole
                        {
                            UserId = professor.UserId,
                            RoleId = profRole.RoleId,
                            SectionId = (int)newSection.SectionId
                        };

                        _lastResponse = _backendHttpClient.Post($"api/v2/UserSectionRole", enrollment);
                        enrollment = JsonConvert.DeserializeObject<UserSectionRole>(_lastResponse.Response);

                        await _backgroundService1?.ReadAndUpdateDB(newSection.SectionId, professor);

                        return Ok("ID " + newSection.SectionId + " enrollment was created");
                    }
                    else
                    {
                        return Conflict(new { message = $"A course already exits with this code {courseDetails.courseCode}" });
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
