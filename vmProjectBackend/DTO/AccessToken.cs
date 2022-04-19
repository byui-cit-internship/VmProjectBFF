namespace vmProjectBackend.DTO
{
    public class AccessToken
    {
        public string AccessTokenValue { get; set; }
        public string CookieValue { get; set; }
        public string CookieName { get; set; }
        public string SiteFrom { get; set; }

        public AccessToken(string accessTokenValue)
        {
            AccessTokenValue = accessTokenValue;
        }
    }
}