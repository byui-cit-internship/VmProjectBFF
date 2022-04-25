using System;

namespace vmProjectBackend.Models
{
    public class VmTemplate
    {
        public int VmTemplateId { get; set; }
        public string VmTemplateVcenterId { get; set; }
        public string VmTemplateName { get; set; }
        public DateTime VmTemplateAccessDate { get; set; }
    }
}