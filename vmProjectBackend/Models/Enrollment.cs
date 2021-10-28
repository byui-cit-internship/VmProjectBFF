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
        public string Status { get; set; }

        //      Reference the connection made with enrollment and user and course
        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}