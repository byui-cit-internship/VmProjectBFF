using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace vmProjectBackend.Models
{
    public class VmTable
    {
        [Key]
        public int VmTableID { get; set; }

        [Required]
        public string VmName { get; set; }

        public string VmFolder { get; set; }

        public string VmResourcePool { get; set; }

    }
}
