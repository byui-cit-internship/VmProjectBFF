﻿namespace vmProjectBackend.DTO
{
    public class ResourceGroup
    {
        public int ResourceGroupId { get; set; }

        public string ResourceGroupName { get; set; }

        public string ResourceGroupVsphereId { get; set; }

        public double Memory { get; set; }

        public double Cpu { get; set; }

    }
}
