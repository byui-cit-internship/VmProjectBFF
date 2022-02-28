using System;
using System.ComponentModel.DataAnnotations;

namespace vmProjectBackend.Models
{
    public class VmSpecification {
        
        [Key]
        public Guid VmSpecification_id { get;set;}
        
        public string spec_name { get;set;}

        public string placement_datastore { get;set;}

        public string placement_folder { get;set;}

        public string placement_resource_pool { get;set;}

        public string memory_size_MiB { get;set;}

        public string cpu_count { get;set;}

        public string  cpu_cores_per_socket { get;set;}

        public string cdroms_iso_file { get;set;}

        public string disks_capacity { get;set;}

        
    }
}

