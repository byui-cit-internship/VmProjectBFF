namespace VmProjectBFF.DTO.Database
{
    public class VmTemplate
    {
        public int VmTemplateId { get; set; }
        public string VmTemplateVCenterId { get; set; }
        public string VmTemplateVCenterName { get; set; }
        public DateTime VmTemplateAccessDate { get; set; }
        public string LibraryVCenterId { get; set; }
    }
}