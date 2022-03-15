using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace vmProjectBackend.Models
{
    public class VmTable
    {
        [Key]
        public Guid VmTableID { get; set; }
        [Required]
        public string VmName { get; set; }

        public string VmFolder { get; set; }

        public string VmResourcePool { get; set; }

        // public int CourseID { get; set; }
        // public int section_num { get; set; }
        // make reference to the course table
        // public ICollection<VmTableCourse> VmTableCourses { get; set; }

    }
}