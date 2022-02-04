using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("group", Schema = "vmProject")]
    public class Group
    {
        [Key]
        [Column("group_id", Order = 1)]
        public int GroupId { get; set; }

        [Required]
        [Column("canvas_group_id", Order = 2)]
        public int CanvasGroupId { get; set; }

        [Required]
        [Column("group_name", TypeName = "varchar(45)", Order = 3)]
        public string GroupName { get; set; }

        [Required]
        [Column("section_id", Order = 4)]
        public int SectionId { get; set; }


        [ForeignKey("SectionId")]
        public Section Section { get; set; }
    }
}