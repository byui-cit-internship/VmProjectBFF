namespace vmProjectBackend.Models
{
    public class CourseCreate
    {
        public string courseName { get; set; }
        public long course_id { get; set; }
        public long userId { get; set; }
        public long VmTable { get; set; }
        public string section_num { get; set; }
        public string canvas_token { get; set; }
    }
}