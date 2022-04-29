namespace vmProjectBFF.DTO.ModelBased
{
    public class GroupMembership
    {
        // Primary Key
        public int GroupMembershipId { get; set; }

        // Not Null
        // Links to Group
        public int GroupId { get; set; }

        // Not Null
        // Links to User
        public int UserId { get; set; }
    }
}
