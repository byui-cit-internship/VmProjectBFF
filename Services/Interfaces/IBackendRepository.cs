using vmProjectBFF.DTO;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public interface IBackendRepository
    {
        public IBackendHttpClient BackendHttpClient { get; }

        public dynamic GetSectionBySemester(string semester);
        public List<Course> GetCoursesByUserId(int userId);
        public List<SectionDTO> GetSectionsByUserId(int userId);
        public List<Semester> GetAllSemesters(int userId);
        public List<User> GetUsersBySection(int sectionId);
        public dynamic getInstancesByUser(int userId);
    }
}
