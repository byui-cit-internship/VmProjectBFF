using VmProjectBFF.DTO;
using Newtonsoft.Json;
using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public dynamic GetSectionBySemester(int semester)
        {
            _lastResponse = _backendHttpClient.Get($"api/v1/section/sectionList", new() { { "semester", semester } });
            return JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response);
        }

        public List<Course> GetCoursesByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Course", new() { { "userId", userId } });
            return JsonConvert.DeserializeObject<List<Course>>(_lastResponse.Response);
        }

        public List<Section> GetSectionsByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "userId", userId } });
            return JsonConvert.DeserializeObject<List<Section>>(_lastResponse.Response);
        }

        public List<Semester> GetAllSemesters(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Semester", new() { { "userId", userId } });
            return JsonConvert.DeserializeObject<List<Semester>>(_lastResponse.Response);
        }

        public List<User> GetUsersBySection(int sectionId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/User", new() { { "sectionId", sectionId } });
            return JsonConvert.DeserializeObject<List<User>>(_lastResponse.Response);
        }

        public dynamic GetInstancesByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get("api/v2/VmInstance", new() { { "userId", userId } });
            List<VmInstance> vmInstances = JsonConvert.DeserializeObject<List<VmInstance>>(_lastResponse.Response);
            List<int> vmTemplateIds = (from instance in vmInstances
                                       select instance.VmTemplateId).Distinct().ToList();
            List<VmTemplate> vmTemplates = new();
            foreach (int vmTemplateId in vmTemplateIds)
            {
                _lastResponse = _backendHttpClient.Get("api/v2/VmTemplate", new() { { "vmTemplateId", vmTemplateId } });
                vmTemplates.Add(JsonConvert.DeserializeObject<VmTemplate>(_lastResponse.Response));
            }
            List<CourseTemplateDTO> courses = new();
            foreach (int vmTemplateId in vmTemplateIds)
            {
                _lastResponse = _backendHttpClient.Get("api/v2/Course", new() { { "vmTemplateId", vmTemplateId } });
                CourseTemplateDTO course = JsonConvert.DeserializeObject<CourseTemplateDTO>(_lastResponse.Response);
                course.VmTemplateId = vmTemplateId;
                courses.Add(course);
            }
            return (
                (from vi in vmInstances
                 join vt in vmTemplates
                 on vi.VmTemplateId equals vt.VmTemplateId
                 join c in courses
                 on vt.VmTemplateId equals c.VmTemplateId
                 select new
                 {
                     CourseCode = c.CourseCode,
                     VmTemplateName = vt.VmTemplateName,
                     VmInstanceExpireDate = vi.VmInstanceExpireDate
                 }).ToList()
            );
        }

        public CreateVmDTO GetCreateVmByEnrollmentId(int enrollmentId)
        {
            _lastResponse = _backendHttpClient.Get("api/v1/CreateVm", new() { { "enrollmentId", enrollmentId } });
            return JsonConvert.DeserializeObject<List<CreateVmDTO>>(_lastResponse.Response).FirstOrDefault();
        }

        public Section GetSectionsByEnrollmentId(int enrollmentId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "userSectionRoleId", enrollmentId } });
            return JsonConvert.DeserializeObject<Section>(_lastResponse.Response);
        }

        public ResourcePool GetResourcePoolByResourcePoolId(int resourcePoolId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/ResourcePool", new() { { "resourcePoolId", resourcePoolId } });
            return JsonConvert.DeserializeObject<ResourcePool>(_lastResponse.Response);
        }

        public Folder GetFolderByFolderId(int folderId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Folder", new() { { "folderId", folderId } });
            return JsonConvert.DeserializeObject<Folder>(_lastResponse.Response);
        }

        public VmTemplate GetTemplateByVCenterId(string vCenterId)
        {
            _lastResponse = _backendHttpClient.Get("api/v2/VmTemplate", new() { { "vmTemplateVcenterId", vCenterId } });
            return JsonConvert.DeserializeObject<VmTemplate>(_lastResponse.Response);
        }

        public List<CourseListByUserDTO> GetStudentCourseByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v1/StudentCourse", new() { { "queryUserId", userId } });
            return JsonConvert.DeserializeObject<List<CourseListByUserDTO>>(_lastResponse.Response);
        }

        public User GetUserByEmail(string email)
        {
            _lastResponse = _backendHttpClient.Get("api/v2/User", new() { { "email", email } });
            return JsonConvert.DeserializeObject<User>(_lastResponse.Response);
        }

        public List<User> GetAdmins()
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/user", new() { { "isAdmin", true } });
            return JsonConvert.DeserializeObject<List<User>>(_lastResponse.Response);
        }
        public List<User> GetProfessors()
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/user", new() { { "role", "professor" } });
            return JsonConvert.DeserializeObject<List<User>>(_lastResponse.Response);
        }
    }
}