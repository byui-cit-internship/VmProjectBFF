namespace VmProjectBFF.DTO.Database
{
    public class PoolMembership
    {
        // Primary Key
        public int PoolMembershipId { get; set; }

        // Not Null
        // Links to Pool
        public int PoolId { get; set; }

        // Not Null
        // Links to User
        public int UserId { get; set; }
    }
}
