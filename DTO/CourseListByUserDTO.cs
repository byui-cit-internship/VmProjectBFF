namespace vmProjectBFF.DTO
{
    public class CourseListByUserDTO
    {
        public int canvasSectionId { get; set; }
        public string sectionName { get; set; }
        public int enrollmentId { get; set; }
        public string studentFullName { get; set; }
        public string vcenterTemplateId { get; set; }

        public CourseListByUserDTO(int canvasSectionId, string sectionName, int enrollmentId, string studentFullName, string vcenterTemplateId)
        {
            this.canvasSectionId = canvasSectionId;
            this.sectionName = sectionName;
            this.enrollmentId = enrollmentId;
            this.studentFullName = studentFullName;
            this.vcenterTemplateId = vcenterTemplateId;
        }
    }
}