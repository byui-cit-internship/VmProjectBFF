using System.ComponentModel.DataAnnotations;

namespace vmProjectBackend.Models
{
    public class Token
    {
        public string ID { get; set; }
        [Required]
        public string token { get; set; }
    }
}