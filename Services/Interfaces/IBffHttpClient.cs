using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public interface IBffHttpClient
    {
        public BffResponse Delete(
            string path,
            dynamic content);

        public BffResponse Get(string path);

        public BffResponse Get(
            string path,
            Dictionary<string, dynamic> queryParams);

        public BffResponse Post(
            string path,
            dynamic content);

        public BffResponse Post(
            string path,
            Dictionary<string, dynamic> queryParams,
            dynamic content);

        public BffResponse Put(
            string path,
            dynamic content);
    }
}
