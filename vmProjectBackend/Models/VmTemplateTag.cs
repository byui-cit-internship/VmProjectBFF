using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("vm_template_tag", Schema = "vmProject")]
    public class VmTemplateTag
    {
        [Key]
        [Column("vm_template_tag_id", Order = 1)]
        public int VmTemplateTagId { get; set; }

        [Required]
        [Column("tag_id", Order = 2)]
        public int TagId { get; set; }

        [Required]
        [Column("vm_template_id", Order = 3)]
        public int VmTemplateId { get; set; }


        [ForeignKey("TagId")]
        public Tag Tag { get; set; }

        [ForeignKey("VmTemplateId")]
        public VmTemplate VmTemplate { get; set; }
    }
}