using System.Text;
using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public class VCenterHttpClient : BffHttpClient, IDisposable
    {
        protected Timer _timer;
        protected bool _disposed = false;

        protected static string GetBaseUrl(IConfiguration configuration)
        {
            return configuration.GetConnectionString("VCenterRootUri");
        }

        protected static string GetVCenterLoginBase64(IConfiguration configuration)
        {
            string vCenterLoginUsername = configuration.GetConnectionString("vCenterLoginUsername");
            string vCenterLoginPassword = configuration.GetConnectionString("vCenterLoginPassword");

            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{vCenterLoginUsername}:{vCenterLoginPassword}"));
        }

        protected static void LogoutLogin(object state)
        {
            WeakReference<VCenterHttpClient> wr = (WeakReference<VCenterHttpClient>)state;
            if (wr.TryGetTarget(out VCenterHttpClient vCenterHttpClient))
            {
                vCenterHttpClient.Logout();
                vCenterHttpClient.Login();
            }
        }

        public VCenterHttpClient(
            IConfiguration configuration,
            ILogger logger)
            : base(
                  GetBaseUrl(configuration),
                  new { Authorization = GetVCenterLoginBase64(configuration) },
                  logger)
        {
            Login();
            _timer = new(LogoutLogin, new WeakReference<VCenterHttpClient>(this), 0, 60 * 1000);

        }

        ~VCenterHttpClient()
        {
            Logout();
            Dispose();
        }

        new protected virtual void Dispose()
        {
            if (!_disposed)
            {
                _timer.Dispose();
                _disposed = true;
            }
        }

        protected void Login()
        {
            try
            {
                BffResponse loginResponse = base.Post("api/session",
                                                      null);
                Headers = new() { { "vmware-api-session-id", loginResponse.Response } };
            }
            catch (BffHttpException ex)
            {
                throw (VCenterHttpException)ex;
            }
        }

        protected void Logout()
        {
            try
            {
                base.Delete("api/session",
                            null);
            }
            catch (BffHttpException ex)
            {
                throw (VCenterHttpException)ex;
            }
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
                throw (VCenterHttpException)ex;
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
                throw (VCenterHttpException)ex;
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
                throw (VCenterHttpException)ex;
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
                throw (VCenterHttpException)ex;
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
                throw (VCenterHttpException)ex;
            }
        }
    }
}
