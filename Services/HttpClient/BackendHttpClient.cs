using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public class BackendHttpClient : BffHttpClient, IBackendHttpClient
    {
        private string _cookie;

        public string Cookie
        {
            get { return _cookie; }
            set { _cookie = $"vima-cookie={value}"; 
            _headers.Add("Cookie", _cookie);  
            }
        }

        protected static string GetBaseUrl(IConfiguration configuration)
        {
            return configuration.GetConnectionString("BackendRootUri");
        }

        protected static object GetVimaCookie(IHttpContextAccessor httpContextAccessor)
        {
            string vimaCookie = null;
            httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("vima-cookie", out vimaCookie);
            return vimaCookie != null ? new { Cookie = $"vima-cookie={vimaCookie}" } : null;
        }

        public BackendHttpClient(
            IConfiguration configuration,
            ILogger<BackendHttpClient> logger,
            IHttpContextAccessor httpContextAccessor)
            : base(
                  GetBaseUrl(configuration),
                  GetVimaCookie(httpContextAccessor),
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
