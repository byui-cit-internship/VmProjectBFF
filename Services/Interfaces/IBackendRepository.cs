using vmProjectBFF.DTO;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public interface IBackendRepository
    {
        public IBackendHttpClient BackendHttpClient { get; }

        // GET's
        public dynamic GetSectionBySemester(string semester);
        public List<Course> GetCoursesByUserId(int userId);
        public List<SectionDTO> GetSectionsByUserId(int userId);
        public List<Semester> GetAllSemesters(int userId);
        public List<User> GetUsersBySection(int sectionId);
        public dynamic GetInstancesByUserId(int userId);
        public CreateVmDTO GetCreateVmByEnrollmentId(int enrollmentId);
        public VmTemplate GetTemplateByVCenterId(string vCenterId);

        // POST's
        public VmInstance CreateVmInstance(VmInstance vmInstance);
    }
}
