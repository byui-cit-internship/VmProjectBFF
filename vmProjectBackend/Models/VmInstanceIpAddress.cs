using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vm_instance_ip_address", Schema = "vmProject")]
    public class VmInstanceIpAddress
    {
        [Key]
        [Column("vm_instance_ip_address_id", Order = 1)]
        public int VmInstanceIpAddressId { get; set; }

        [Required]
        [Column("vm_instance_id", Order = 2)]
        public int VmInstanceId { get; set; }

        [Required]
        [Column("ip_address_id", Order = 3)]
        public int IpAddressId { get; set; }


        [ForeignKey("VmInstanceId")]
        public VmInstance VmInstance { get; set; }

        [ForeignKey("IpAddressId")]
        public IpAddress IpAddress { get; set; }
    }
}