using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace vmProjectBackend.Models
{
    public class User

    {
        [Key]
        public long UserID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        [Required]
        public string userType { get; set; }

        //  if a given Student row in the database has two related Enrollment rows (rows that contain that student's primary key value in their StudentID 
        // foreign key column), that Student entity's Enrollments navigation property will contain those two Enrollment entities.
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    //     public virtual ICollection<Course> Courses { get; set; }
     }



}