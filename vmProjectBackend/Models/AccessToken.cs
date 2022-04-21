using System;


namespace vmProjectBackend.Models
{
    public class AccessToken
    {
        public int AccessTokenId { get; set; }

        public string AccessTokenValue { get; set; }

        public DateTime ExpireDate { get; set; }

        public int UserId { get; set; }

    }
}