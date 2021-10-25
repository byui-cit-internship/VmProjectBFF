using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace vmProjectBackend.Models
{
    public class Course
    {
        // should we set courseCode to the primary key
        // this declarator makes us define the courseId instead of the database generating it

        [Key]

        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CourseID { get; set; }
        public string CourseName { get; set; }
        public string section_num { get; set; }

        public string canvas_token { get; set; }

        public string description { get; set; }
        public string semester { get; set; }

        // this below is the can be emmited since enrollment will connect them

        // public long UserId { get; set; }
        // this is for course connection ot enrollment
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        // this is course to vmtables
        public virtual ICollection<VmTableCourse> VmTableCourses { get; set; }
        // public virtual User User { get; set; }



    }
}