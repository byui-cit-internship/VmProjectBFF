using System;
using System.ComponentModel.DataAnnotations;

namespace vmProjectBackend.Models
{
    public class VmUtilization {
        
        [Key]
        public Guid UtilizationID { get;set; }
        
        public string StudentName { get;set; }

        public string StudentEmail { get;set; }

        public string VirtualMachine { get; set; }

        public DateTime CreationDate { get; set; }

        
    }
}

