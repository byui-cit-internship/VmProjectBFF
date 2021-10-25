using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace vmProjectBackend.Models
{
    public class VmTableCourse
    {
        public int VmTableCourseID { get; set; }

        public long CourseID { get; set; }
        public Course Course { get; set; }
        public long VmTableID { get; set; }
        public VmTable VmTable { get; set; }

    }
}