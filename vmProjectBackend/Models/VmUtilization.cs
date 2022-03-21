using System;
using System.ComponentModel.DataAnnotations;

namespace vmProjectBackend.Models
{
    public class VmUtilization
    {

        [Key]
        public Guid UtilizationID { get; set; }

        public string StudentName { get; set; }

        public string StudentEmail { get; set; }

        public string VirtualMachine { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
    }
}