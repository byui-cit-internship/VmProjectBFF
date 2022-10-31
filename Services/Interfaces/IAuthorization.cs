using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.Services
{
    public interface IAuthorization
    {
        public User GetAuth(
            string authType,
            int? sectionId = null);
    }
}
