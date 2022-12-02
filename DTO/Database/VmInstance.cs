namespace VmProjectBFF.DTO.Database
{
    public class VmInstance
    {
        public int VmInstanceId { get; set; }
        public int VmTemplateId { get; set; }
        public string VmInstanceVcenterId { get; set; }
        public DateTime VmInstanceExpireDate { get; set; }
        public string VmInstanceVcenterName { get; set; }
    }
}
