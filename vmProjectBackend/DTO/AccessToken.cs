namespace vmProjectBackend.DTO
{
    public class AccessToken
    {
        public string AccessTokenValue { get; set; }

        public AccessToken(string accessTokenValue)
        {
            AccessTokenValue = accessTokenValue;
        }
    }
}