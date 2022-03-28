<<<<<<< HEAD
using System;
=======
﻿using System;
>>>>>>> auth-ebe
using System.ComponentModel.DataAnnotations;

namespace vmProjectBackend.Models
{
<<<<<<< HEAD
    public class VmDetail {
        [Key]
        public Guid VmDetailsID { get;set;}
        
        public string Template_id { get;set;}

        public string Enrollment_id { get;set;}

        public string Course_id { get;set;}

        public string User_id { get;set;}
=======
    public class VmDetail
    {
        [Key]
        public Guid VmDetailsID { get; set; }

        public string Template_id { get; set; }

        public string Enrollment_id { get; set; }

        public string Course_id { get; set; }

        public string User_id { get; set; }
>>>>>>> auth-ebe
    }
}