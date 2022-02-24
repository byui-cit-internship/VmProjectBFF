using System;
using System.ComponentModel.DataAnnotations;

namespace vmProjectBackend.Models
{
    public class VmSpecification {
        
        public Guid Vm_id { get;set;}
        
        public string Vm_name { get;set;}

        public string Vm_cores { get;set;}

        public string Vm_memory { get;set;}

        public string Vm_storage { get;set;}
    }
}