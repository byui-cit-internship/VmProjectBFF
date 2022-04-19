namespace vmProjectBackend.DTO
{
    public class CourseListByUserDTO
    {
        private int canvasSectionId;
        private string courseName;
        private int enrollmentId;
        private string studentFullName;
        private string vcenterTemplateId;

        public CourseListByUserDTO(int canvasSectionId, string courseName, int enrollmentId, string studentFullName, string vcenterTemplateId)
        {
            this.canvasSectionId = canvasSectionId;
            this.courseName = courseName;
            this.enrollmentId = enrollmentId;
            this.studentFullName = studentFullName;
            this.vcenterTemplateId = vcenterTemplateId;
        }
    }
}