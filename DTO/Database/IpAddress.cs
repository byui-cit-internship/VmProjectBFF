namespace VmProjectBFF.DTO.Database
{
    public class IpAddress
    {
        // Primary Key
        public int IpAddressId { get; set; }

        // Not Null
        public byte[] IpAddressField { get; set; }

        // Not Null
        public byte[] SubnetMask { get; set; }

        // Not Null
        public bool IsIpv6 { get; set; }
    }
}
