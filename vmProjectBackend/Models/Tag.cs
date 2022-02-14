using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("tag", Schema = "vmProject")]
    public class Tag
    {
        [Key]
        [Column("tag_id", Order = 1)]
        public int TagId { get; set; }

        [Required]
        [Column("tag_category_id", Order = 2)]
        public int TagCategoryId { get; set; }

        [Required]
        [Column("tag_vcenter_id", TypeName = "varchar(100)", Order = 3)]
        public string TagVcenterId { get; set; }

        [Required]
        [Column("tag_name", TypeName = "varchar(45)", Order = 4)]
        public string TagName { get; set; }

        [Column("tag_description", TypeName = "varchar(100)", Order = 5)]
        public string TagDescription { get; set; }


        [ForeignKey("TagCategoryId")]
        public TagCategory TagCategory { get; set; }
    }
}