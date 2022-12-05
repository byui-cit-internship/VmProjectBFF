namespace VmProjectBFF.DTO.Database
{
    public class AccessToken
    {
        public int AccessTokenId { get; set; }

        public string AccessTokenValue { get; set; }

        public DateTime ExpireDate { get; set; }

        public int UserId { get; set; }

        public AccessToken(string accessTokenValue)
        {
            AccessTokenValue = accessTokenValue;
        }
    }
}