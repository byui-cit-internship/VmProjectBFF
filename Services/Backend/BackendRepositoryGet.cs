using vmProjectBFF.DTO;
using System.Text.Json;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public List<OldSectionDTO> GetSectionBySemester(string semester)
        {
            _lastResponse = _backendHttpClient.Get($"api/v1/section/sectionList/{semester}");
            return JsonSerializer.Deserialize<List<OldSectionDTO>>(_lastResponse.Response);
        }

        public List<Course> GetCoursesByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Course", new() { { "userId", userId } });
            return JsonSerializer.Deserialize<List<Course>>(_lastResponse.Response);
        }

        public List<SectionDTO> GetSectionsByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "userId", userId } });
            return JsonSerializer.Deserialize<List<SectionDTO>>(_lastResponse.Response);
        }
    }
}