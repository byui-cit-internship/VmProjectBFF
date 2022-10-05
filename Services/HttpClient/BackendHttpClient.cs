using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public class BackendHttpClient : BffHttpClient
    {
        private string _cookie;

        public string Cookie
        {
            get { return _cookie; }
            set { _cookie = value; }
        }

        protected static string GetBaseUrl(IConfiguration configuration)
        {
            return configuration.GetConnectionString("BackendRootUri");
        }

        protected static object GetVimaCookie(string vimaCookieValue)
        {
            return vimaCookieValue != null ? new { Cookie = $"vima-cookie={vimaCookieValue}" } : null;
        }

        public BackendHttpClient(
            IConfiguration configuration,
            ILogger logger,
            string vimaCookieValue)
            : base(
                  GetBaseUrl(configuration),
                  GetVimaCookie(vimaCookieValue),
                  logger)
        {
        }

        public override BffResponse Delete(
            string path,
            dynamic content)
        {
            try
            {
                return base.Delete(path,
                                   (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new BackendHttpException(ex);
            }
        }

        public override BffResponse Get(string path)
        {
            try
            {
                return base.Get(path);
            }
            catch (BffHttpException ex)
            {
                throw new BackendHttpException(ex);
            }
        }

        public override BffResponse Get(
            string path,
            object queryParams)
        {
            try
            {
                return base.Get(path,
                                queryParams);
            }
            catch (BffHttpException ex)
            {
                throw new BackendHttpException(ex);
            }
        }

        public override BffResponse Post(
            string path,
            dynamic content)
        {
            try
            {
                return base.Post(path,
                                 (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new BackendHttpException(ex);
            }
        }

        public override BffResponse Put(
            string path,
            dynamic content)
        {
            try
            {
                return base.Put(path,
                                 (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new BackendHttpException(ex);
            }
        }
    }
}
