namespace vmProjectBackend.DTO
{
    public class CourseListByUserDTO
    {
        public int canvasSectionId { get; set; }
        public string courseName { get; set; }
        public int enrollmentId { get; set; }
        public string studentFullName { get; set; }
        public string vcenterTemplateId { get; set; }

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