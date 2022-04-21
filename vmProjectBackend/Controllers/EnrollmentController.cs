using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using vmProjectBackend.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using vmProjectBackend.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly Authorization _auth;
        private readonly Backend _backend;
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EnrollmentController> _logger;
        private readonly BackgroundService1 _bs1;
        private readonly IServiceScope _scope;
        private BackendResponse _lastResponse;

        public EnrollmentController(
            DatabaseContext context,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<EnrollmentController> logger,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _auth = new(_backend, _context, _logger);
            _httpClientFactory = httpClientFactory;
            // _scope = scope;
        }

        // [HttpPost("backgroundservice")]
        // public async Task<ActionResult> RunBackgroundService(){
        //     IServiceProvider services = _scope.ServiceProvider;
        //     BackgroundService1 backgroundService = services.GetRequiredService<BackgroundService1>();
        //     await backgroundService.ReadAndUpdateDB();
        //     return Ok();
        // }


        /****************************************
        Allows professor to create a course in the database using a canvas course id, a course name,
        a description, a canavs api token, a section number, a semester, and vm template id
        ****************************************/
        [HttpPost("professor/register/course")]
        public async Task<ActionResult<CourseCreate>> CreateCourseEnrollment([FromBody] CourseCreate courseDetails)
        {
            // Gets email from session
            string userEmail = HttpContext.User.Identity.Name;

            // Returns a professor user or null if email is not associated with a professor
            User professor = _auth.getAuth("admin");

            if (professor != null && courseDetails != null)
            {
                // Check if a course already exists

                _lastResponse = _backend.Get($"api/v2/Section", new { sectionCanvasId = courseDetails.canvasCourseId });
                SectionDTO courseExist = JsonConvert.DeserializeObject<SectionDTO>(_lastResponse.Response);

                // If not, create course
                if (courseExist == null)
                {
                    _lastResponse = _backend.Get($"api/v2/Course", new { courseName = courseDetails.courseName });
                    CourseDTO course = JsonConvert.DeserializeObject<CourseDTO>(_lastResponse.Response);

                    if (course == null)
                    {

                        _lastResponse = _backend.Get($"api/v2/ResourceGroupTemplate", new { memory = 0, cpu = 0 });
                        ResourceGroup resourceGroupTemplate = JsonConvert.DeserializeObject<ResourceGroup>(_lastResponse.Response);
                        if (resourceGroupTemplate == null)
                        {
                            resourceGroupTemplate = new(0, 0);
                            _lastResponse = _backend.Post("api/v2/ResourceGroupTemplate", resourceGroupTemplate);
                            resourceGroupTemplate = JsonConvert.DeserializeObject<ResourceGroup>(_lastResponse.Response);
                        }

                        // Create new course
                        course = new();
                        course.CourseName = courseDetails.courseName;
                        course.CourseCode = courseDetails.courseName;
                        course.ResourceGroupTemplateId = resourceGroupTemplate.ResourceGroupTemplateId;

                        _lastResponse = _backend.Post("api/v2/Course", course);
                        course = JsonConvert.DeserializeObject<CourseDTO>(_lastResponse.Response);
                    }

                    _lastResponse = _backend.Get($"api/v2/Folder", new { vcenterFolderId = courseDetails.folder });
                    Folder folder = JsonConvert.DeserializeObject<Folder>(_lastResponse.Response);

                    if (folder == null)
                    {
                        folder = new();
                        folder.VcenterFolderId = courseDetails.folder;
                        _lastResponse = _backend.Post($"api/v2/Folder", folder);
                        folder = JsonConvert.DeserializeObject<Folder>(_lastResponse.Response);
                    }

                    /*
                    course.CanvasCourseId = courseDetails.canvasCourseId;
                    course.CourseName = courseDetails.courseName;
                    //course.ContentLibrary = courseDetails.contentLibrary; // why here
                    course.TemplateVm = courseDetails.templateVm;
                    //course.Semester = courseDetails.semester; // shouldn't be nessesary
                    //course.Description = courseDetails.description;
                    course.Folder = courseDetails.folder;
                    course.Resource_pool = courseDetails.resource_pool;
                    course.Section = courseDetails.section;
                    _context.Courses.Add(course);
                    _context.SaveChanges();
                    */

                    // Update professor's canvas api token

                    professor.CanvasToken = courseDetails.canvas_token;
                    _lastResponse = _backend.Put("api/v2/User", professor);
                    professor = JsonConvert.DeserializeObject<User>(_lastResponse.Response);

                    // Return a semester from the database using provided semester term
                    _lastResponse = _backend.Get($"api/v2/Semester", new { semesterTerm = courseDetails.semester, semesterYear = 2022 });
                    Semester term = JsonConvert.DeserializeObject<Semester>(_lastResponse.Response);

                    _lastResponse = _backend.Get($"api/v2/ResourceGroupTemplate", new { resourceGroupTemplateId = course.ResourceGroupTemplateId });
                    ResourceGroup resourceGroup = JsonConvert.DeserializeObject<ResourceGroup>(_lastResponse.Response);


                    _lastResponse = _backend.Post($"api/v2/ResourceGroup", resourceGroup);
                    resourceGroup = JsonConvert.DeserializeObject<ResourceGroup>(_lastResponse.Response);

                    // If no semester exists, make a semester.
                    if (term == null)
                    {
                        term = new Semester();
                        term.SemesterTerm = courseDetails.semester;
                        term.SemesterYear = 2022;
                        term.StartDate = new DateTime(2022, 1, 1);
                        term.EndDate = new DateTime(2022, 12, 31);

                        _lastResponse = _backend.Post($"api/v2/Semester", term);
                        term = JsonConvert.DeserializeObject<Semester>(_lastResponse.Response);
                    }

                    // Return a vm template from the database using the provided vsphere template id
                    _lastResponse = _backend.Get($"api/v2/VmTemplate", new { vmTemplateVcenterId = courseDetails.templateVm });
                    VmTemplate template = JsonConvert.DeserializeObject<VmTemplate>(_lastResponse.Response);

                    // If template doesn't exist, create it
                    if (template == null)
                    {
                        template = new VmTemplate();
                        template.VmTemplateVcenterId = courseDetails.templateVm;
                        template.VmTemplateName = "test";
                        template.VmTemplateAccessDate = new DateTime(2022, 1, 1);

                        _lastResponse = _backend.Post($"api/v2/VmTemplate", template);
                        template = JsonConvert.DeserializeObject<VmTemplate>(_lastResponse.Response);
                    }

                    // Create section in database using provided canvas course id and section number,
                    // along with previous course and term
                    SectionDTO newSection = new();
                    newSection.CourseId = (int)course.CourseId;
                    newSection.SectionCanvasId = Int32.Parse(courseDetails.canvasCourseId);
                    newSection.SemesterId = term.SemesterId;
                    newSection.SectionNumber = courseDetails.section_num;
                    newSection.FolderId = folder.FolderId;
                    newSection.ResourceGroupId = resourceGroup.ResourceGroupId;

                    _lastResponse = _backend.Post($"api/v2/Section", newSection);
                    newSection = JsonConvert.DeserializeObject<SectionDTO>(_lastResponse.Response);

                    // Get role to signify that the person craeting this section is a professor
                    _lastResponse = _backend.Get($"api/v2/Role", new { roleName = "Professor" });
                    Role profRole = JsonConvert.DeserializeObject<List<Role>>(_lastResponse.Response).FirstOrDefault();

                    if (profRole == null)
                    {
                        profRole = new();
                        profRole.RoleName = "Professor";
                        profRole.CanvasRoleId = 56898;

                        _lastResponse = _backend.Post($"api/v2/Role", profRole);
                        profRole = JsonConvert.DeserializeObject<Role>(_lastResponse.Response);
                    }

                    _lastResponse = _backend.Get($"api/v2/TagCategory", new { tagCategoryName = "Course" });
                    TagCategory tagCategory = JsonConvert.DeserializeObject<TagCategory>(_lastResponse.Response);

                    if (tagCategory == null)
                    {
                        tagCategory = new();
                        tagCategory.TagCategoryVcenterId = "TEMP_USE";
                        tagCategory.TagCategoryName = "Course";
                        tagCategory.TagCategoryDescription = "Do not attempt to use with VCenter.";

                        _lastResponse = _backend.Post($"api/v2/TagCategory", tagCategory);
                        tagCategory = JsonConvert.DeserializeObject<TagCategory>(_lastResponse.Response);
                    }

                    _lastResponse = _backend.Get($"api/v2/Tag", new { tagCategoryId = tagCategory.TagCategoryId, tagName = course.CourseCode });
                    Tag tag = JsonConvert.DeserializeObject<Tag>(_lastResponse.Response);

                    if (tag == null)
                    {
                        tag = new();
                        tag.TagVcenterId = "TEMP_USE";
                        tag.TagCategoryId = tagCategory.TagCategoryId;
                        tag.TagName = course.CourseCode;
                        tag.TagDescription = "Do not attempt to use with VCenter.";

                        _lastResponse = _backend.Post($"api/v2/Tag", tag);
                        tag = JsonConvert.DeserializeObject<Tag>(_lastResponse.Response);
                    }

                    _lastResponse = _backend.Get($"api/v2/VmTemplateTag", new { tagId = tag.TagId, vmTemplateId = template.VmTemplateId });
                    VmTemplateTag vmTemplateTag = JsonConvert.DeserializeObject<VmTemplateTag>(_lastResponse.Response);

                    if (vmTemplateTag == null)
                    {
                        vmTemplateTag = new();
                        vmTemplateTag.VmTemplateId = template.VmTemplateId;
                        vmTemplateTag.TagId = tag.TagId;

                        _lastResponse = _backend.Post($"api/v2/VmTemplateTag", vmTemplateTag);
                        vmTemplateTag = JsonConvert.DeserializeObject<VmTemplateTag>(_lastResponse.Response);
                    }

                    // Create a link between the created section and the professor, effectively enrolling them
                    // in the class they created.
                    UserSectionRole enrollment = new UserSectionRole
                    {
                        UserId = professor.UserId,
                        RoleId = profRole.RoleId,
                        SectionId = (int)newSection.SectionId
                    };

                    _lastResponse = _backend.Post($"api/v2/UserSectionRole", enrollment);
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
    }
}
