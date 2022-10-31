namespace VmProjectBFF.DTO
{
    public class SessionToken
    {
        public int SessionTokenId { get; set; }
        public Guid SessionTokenValue { get; set; }
        public string SessionCookie { get; set; }
        public DateTime ExpireDate { get; set; }
        public int AccessTokenId { get; set; }
    }
}