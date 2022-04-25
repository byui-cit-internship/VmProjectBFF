namespace vmProjectBackend.DTO
{
    public class AccessTokenDTO
    {
        public string AccessTokenValue { get; set; }
        public string CookieValue { get; set; }
        public string CookieName { get; set; }
        public string SiteFrom { get; set; }

        public AccessTokenDTO(string accessTokenValue)
        {
            AccessTokenValue = accessTokenValue;
        }
    }
}