namespace VmProjectBFF.DTO.Database
{
    public class DeployVmRequirements
    {
        public int enrollment_id { get; set; }

        public string vmInstanceName { get; set; }

        public DateTime vmInstanceCreationDate { get; set; }
    
        public string templateId { get; set;}
    }
}