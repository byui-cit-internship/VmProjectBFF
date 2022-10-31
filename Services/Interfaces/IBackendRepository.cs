using VmProjectBFF.DTO;
using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.Services
{
    public interface IBackendRepository
    {
        public IBackendHttpClient BackendHttpClient { get; }

        // GET's
        public dynamic GetSectionBySemester(string semester);
        public List<Course> GetCoursesByUserId(int userId);
        public List<Section> GetSectionsByUserId(int userId);
        public List<Semester> GetAllSemesters(int userId);
        public List<User> GetUsersBySection(int sectionId);
        public dynamic GetInstancesByUserId(int userId);
        public CreateVmDTO GetCreateVmByEnrollmentId(int enrollmentId);
        public VmTemplate GetTemplateByVCenterId(string vCenterId);
        public List<CourseListByUserDTO> GetStudentCourseByUserId(int userId);
        public User GetUserByEmail(string email);
        public List<User> GetAdmins();

        // POST's
        public VmInstance CreateVmInstance(VmInstance vmInstance);
        public (User, string) PostToken(AccessToken token);
        public User PostUser(User user);

        // PUT's
        public User PutUser(User user);

        // DELETE's
        public void DeleteToken();
    }
}
