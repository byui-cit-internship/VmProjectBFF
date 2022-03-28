<<<<<<< HEAD
using System;
=======
﻿using System;
>>>>>>> auth-ebe
using System.ComponentModel.DataAnnotations;

namespace vmProjectBackend.Models
{
<<<<<<< HEAD
    public class VmUtilization {
        
        [Key]
        public Guid UtilizationID { get;set; }
        
        public string StudentName { get;set; }

        public string StudentEmail { get;set; }
=======
    public class VmUtilization
    {

        [Key]
        public Guid UtilizationID { get; set; }

        public string StudentName { get; set; }

        public string StudentEmail { get; set; }
>>>>>>> auth-ebe

        public string VirtualMachine { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
<<<<<<< HEAD
        public DateTime CreationDate { get; set; }       
    }
}

                                                                                                                                        
=======
        public DateTime CreationDate { get; set; }
    }
}
>>>>>>> auth-ebe
