using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace vmProjectBackend.Models
{
    public class Enrollment
    {
        [Key]
        public long EnrollmentID { get; set; }

        public long CourseID { get; set; }

        public long UserId { get; set; }
        [Required]
        public long teacherId { get; set; }
        public long VmTableID { get; set; }

        //Extra entities for the enrollment.
        public string Status { get; set; }
        public string section_num { get; set; }
        // not sure if this should be in here
        public string canvas_token { get; set; }
        public string semester { get; set; }
        //Reference the connection made with enrollment and user and course
        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
        public virtual VmTable VmTable { get; set; }
    }
}