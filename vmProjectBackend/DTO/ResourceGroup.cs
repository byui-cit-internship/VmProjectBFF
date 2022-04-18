namespace vmProjectBackend.DTO
{
    public class ResourceGroup
    {
        public int? ResourceGroupId { get; set; }

        public double Memory { get; set; }

        public double Cpu { get; set; }

        public ResourceGroup(double memory, double cpu)
        {
            Memory = memory;
            Cpu = cpu;
        }
    }
}
