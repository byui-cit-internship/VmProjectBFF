using vmProjectBFF.DTO;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public interface IBackendRepository
    {
        public IBackendHttpClient BackendHttpClient { get; }

        public List<OldSectionDTO> GetSectionBySemester(string semester);
        public List<Course> GetCoursesByUserId(int userId);
        public List<SectionDTO> GetSectionsByUserId(int userId);
    }
}
