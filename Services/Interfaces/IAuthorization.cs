using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public interface IAuthorization
    {
        public User GetAuth(
            string authType,
            int? sectionId = null)
        {
            return null;
        }
    }
}
