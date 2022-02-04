using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vmProjectBackend.Models
{
    [Table("tag_category", Schema = "vmProject")]
    public class TagCategory
    {
        [Key]
        [Column("tag_category_id", Order = 1)]
        public int TagCategoryId { get; set; }

        [Required]
        [Column("tag_category_vcenter_id", TypeName = "varchar(100)", Order = 2)]
        public string TagCategoryVcenterId { get; set; }

        [Required]
        [Column("tag_category_name", TypeName = "varchar(45)", Order = 3)]
        public string TagCategoryName { get; set; }

        [Column("tag_category_description", TypeName = "varchar(100)", Order = 4)]
        public string TagCategoryDescription { get; set; }
    }
}