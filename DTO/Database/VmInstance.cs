namespace VmProjectBFF.DTO.Database
{
    public class VmInstance
    {
        public string vmTemplateName {get; set;}
        public string VmInstanceId { get; set; }
        public string VmTemplateId { get; set; }
        public string VmInstanceVcenterId { get; set; }
        public DateTime VmInstanceCreationDate { get; set; }
        public DateTime VmInstanceExpireDate { get; set; }
        public string VmInstanceVcenterName { get; set; }
        public int SectionId { get; set; }
        public int UserId { get; set; }
    }
}
