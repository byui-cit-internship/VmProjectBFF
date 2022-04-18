namespace vmProjectBackend.DTO
{
    public class CourseDTO
    {
        public int? CourseId { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public int ResourceGroupTemplateId { get; set; }
    

        public CourseDTO(int? courseId, string courseCode, string courseName, int resourceGroupTemplateId)
        {
            CourseId = courseId;
            CourseCode = courseCode;
            CourseName = courseName;
            ResourceGroupTemplateId = resourceGroupTemplateId;
        }
    }
}
