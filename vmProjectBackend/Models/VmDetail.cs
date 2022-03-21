using System;
using System.ComponentModel.DataAnnotations;

namespace vmProjectBackend.Models
{
    public class VmDetail
    {
        [Key]
        public Guid VmDetailsID { get; set; }

        public string Template_id { get; set; }

        public string Enrollment_id { get; set; }

        public string Course_id { get; set; }

        public string User_id { get; set; }
    }
}