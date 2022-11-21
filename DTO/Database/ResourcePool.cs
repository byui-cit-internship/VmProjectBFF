namespace VmProjectBFF.DTO
{
    public class ResourcePool
    {
        public int ResourcePoolId { get; set; }

        public double Memory { get; set; }

        public double Cpu { get; set; }

        public string ResourcePoolName { get; set; }

        public string ResourcePoolVsphereId { get; set; }
    }
}
