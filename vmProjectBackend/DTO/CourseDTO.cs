namespace vmProjectBackend.DTO
{
    public class CourseDTO
    {
        public int CourseId { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public int? ResourceGroupTemplateId { get; set; }
    

        public CourseDTO(string courseCode, string courseName, int resourceGroupTemplateId)
        {
            CourseCode = courseCode;
            CourseName = courseName;
            ResourceGroupTemplateId = resourceGroupTemplateId;
        }

        public CourseDTO() { }
    }
}
