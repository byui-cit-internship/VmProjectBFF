using System;
using System.ComponentModel.DataAnnotations;

namespace vmProjectBackend.Models
{
    public class VmSpecification {
        
        [Key]
        public Guid VmSpecification_id { get;set;}
        
        public string name { get;set;}

        public string guest_OS { get;set;}

        public string datastore { get;set;}

        public string folder { get;set;}

        public string resource_pool { get;set;}
    }
}