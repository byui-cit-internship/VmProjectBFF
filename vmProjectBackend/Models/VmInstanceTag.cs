using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vm_instance_tag", Schema = "vmProject")]
    public class VmInstanceTag
    {
        [Key]
        [Column("vm_instance_tag_id", Order = 1)]
        public int VmInstanceTagId { get; set; }

        [Required]
        [Column("tag_id", Order = 2)]
        public int TagId { get; set; }

        [Required]
        [Column("vm_instance_id", Order = 3)]
        public int VmInstanceId { get; set; }


        [ForeignKey("TagId")]
        public Tag Tag { get; set; }

        [ForeignKey("VmInstanceId")]
        public VmInstance VmInstance { get; set; }
    }
}