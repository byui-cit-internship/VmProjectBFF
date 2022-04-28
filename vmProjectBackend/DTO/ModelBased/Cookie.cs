namespace vmProjectBackend.DTO
{
    public class Cookie
    {
        // Primary Key
        public int CookieId { get; set; }
        
        // Not Null
        // Links to SessionToken
        public int SessionTokenId { get; set; }
        
        // Not Null
        public string CookieName { get; set; }
        
        // Not Null
        public string CookieValue { get; set; }
        
        // Not Null
        public string SiteFrom { get; set; }
    }
}
