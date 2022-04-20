namespace vmProjectBackend.DTO
{
    public class Cookie
    {
        public int CookieId { get; set; }
        public int SessionTokenId { get; set; }
        public string CookieName { get; set; }
        public string CookieValue { get; set; }
        public string SiteFrom { get; set; }
    }
}
