using VmProjectBFF.DTO;

namespace VmProjectBFF.Services
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
            string path);            

        public BffResponse Post(
            string path,
            Dictionary<string, dynamic> queryParams,
            dynamic content);

        public BffResponse Put(
            string path,
            dynamic content);
    }
}
