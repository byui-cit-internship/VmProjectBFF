using vmProjectBFF.Models;

namespace vmProjectBFF.DTO
{
    public class UserSession
    {
        public User User { get; set; }
        public SessionToken SessionToken { get; set; }

        public UserSession(User user, SessionToken sessionToken)
        {
            User = user;
            SessionToken = sessionToken;
        }
    }
}
