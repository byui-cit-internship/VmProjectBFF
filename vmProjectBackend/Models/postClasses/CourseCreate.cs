using System;

namespace vmProjectBackend.Models
{
    public class CourseCreate
    {
        public int course_id { get; set; }
        public string courseName { get; set; }
        public string description { get; set; }
        public string canvas_token { get; set; }
        public int section_num { get; set; }
        public string semester { get; set; }
        public int section { get; set; }
        public string contentLibrary { get; set; }

        public string folder { get; set; }

        public string templateVm { get; set; }

        public string resource_pool { get; set; }

        public string userId { get; set; }
        public string teacherId { get; set; }
        public string vmTableID { get; set; }
        public string canvasCourseId {get; set;}
    }
}
