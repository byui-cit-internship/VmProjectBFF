using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace vmProjectBackend.Models
{
    public class VmTable
    {
        [Key]
        public long VmTableID {get;set;}
        [Required]
        public string vm_image {get;set;}
    
        public int CourseID { get; set; }
        public int section_num { get; set; }


        // make reference to the course table
        public virtual Course Course {get;set;}



    }
}