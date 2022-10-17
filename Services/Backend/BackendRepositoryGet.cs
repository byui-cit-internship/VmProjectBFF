using vmProjectBFF.DTO;
using Newtonsoft.Json;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public dynamic GetSectionBySemester(string semester)
        {
            _lastResponse = _backendHttpClient.Get($"api/v1/section/sectionList", new { semester = semester });
            return JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response);
        }

        public List<Course> GetCoursesByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Course", new { userId = userId });
            return JsonConvert.DeserializeObject<List<Course>>(_lastResponse.Response);
        }

        public List<SectionDTO> GetSectionsByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new { userId = userId });
            return JsonConvert.DeserializeObject<List<SectionDTO>>(_lastResponse.Response);
        }
    }
}