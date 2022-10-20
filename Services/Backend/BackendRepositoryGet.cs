using vmProjectBFF.DTO;
using Newtonsoft.Json;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public dynamic GetSectionBySemester(string semester)
        {
            _lastResponse = _backendHttpClient.Get($"api/v1/section/sectionList", new() { { "semester", semester } });
            return JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response);
        }

        public List<Course> GetCoursesByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Course", new() { { "userId", userId } });
            return JsonConvert.DeserializeObject<List<Course>>(_lastResponse.Response);
        }

        public List<SectionDTO> GetSectionsByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "userId", userId } });
            return JsonConvert.DeserializeObject<List<SectionDTO>>(_lastResponse.Response);
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
        public dynamic getInstancesByUser(int userId)
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
            List<dynamic> courses = new();
            foreach (int vmTemplateId in vmTemplateIds)
            {
                _lastResponse = _backendHttpClient.Get("api/v2/Course", new() { { "vmTemplateId", vmTemplateId } });
                dynamic course = JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response);
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
    }
}