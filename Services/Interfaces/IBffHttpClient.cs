using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public interface IBffHttpClient
    {
        public BffResponse Delete(
            string path,
            dynamic content)
        {
            return null;
        }

        public BffResponse Get(string path)
        {
            return null;
        }

        public BffResponse Get(
            string path,
            object queryParams)
        {
            return null;
        }

        public BffResponse Post(
            string path,
            dynamic content)
        {
            return null;
        }

        public BffResponse Put(
            string path,
            dynamic content)
        {
            return null;
        }
    }
}
