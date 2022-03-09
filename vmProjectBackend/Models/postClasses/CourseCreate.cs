
// this model is not link to the Database, it is just used to recieve data from post methods,
//  emrollmentController post method: [HttpPost("professor/register/course")]
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
        public Guid userId { get; set; }
        public Guid teacherId { get; set; }
        public string vmTableID { get; set; }
        public string status { get; set; }
    }
}