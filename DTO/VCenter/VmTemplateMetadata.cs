namespace VmProjectBFF.DTO.VCenter
{
    public class VmTemplateMetadata
    {
        public string cpuCount { get; set; }
        public string coresPerSocket { get; set; }
        public string os { get; set; }
        public string memory { get; set; }
        public dynamic storage { get; set; }

    }
}