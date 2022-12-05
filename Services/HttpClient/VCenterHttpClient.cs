using System.Text;
using VmProjectBFF.DTO;
using VmProjectBFF.Exceptions;
using Newtonsoft.Json;
using VcenterToken = VmProjectBFF.DTO.VCenter.SessionToken;

namespace VmProjectBFF.Services
{
    public class VCenterHttpClient : BffHttpClient, IDisposable, IVCenterHttpClient
    {
        protected Timer _timer;
        protected bool _disposed = false;
        protected bool _initialized = false;

        protected static string GetBaseUrl(IConfiguration configuration)
        {
            return configuration.GetConnectionString("VCenterRootUri");
        }

        protected static string GetVCenterLoginBase64(IConfiguration configuration)
        {
            string vCenterLoginUsername = configuration.GetConnectionString("VCenterLoginUsername");
            string vCenterLoginPassword = configuration.GetConnectionString("VCenterLoginPassword");

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
            ILogger<VCenterHttpClient> logger)
            : base(
                  GetBaseUrl(configuration),
                  new { Authorization = $"Basic {GetVCenterLoginBase64(configuration)}" },
                  configuration,
                  logger)
        {
        }

        public void Initialize()
        {
            _initialized = true;
            Login();
            _timer = new(LogoutLogin, new WeakReference<VCenterHttpClient>(this), 60 * 1000, 60 * 1000);
        }

        void IDisposable.Dispose()
        {
            if (!_disposed && _initialized)
            {
                _timer.Dispose();
                Logout();
                _disposed = true;
            }
        }

        protected void Login()
        {
            try
            {
                if (!Headers.ContainsKey("Authorization"))
                {
                    Headers = new() { { "Authorization", $"Basic {GetVCenterLoginBase64(_configuration)}" } };
                }
                BffResponse loginResponse = base.Post("rest/com/vmware/cis/session",
                                                      null);
                VcenterToken sessionToken = JsonConvert.DeserializeObject<VcenterToken>(loginResponse.Response);
                _logger.LogInformation($"{sessionToken.value}");
                Headers = new() { { "Cookie", $"vmware-api-session-id={sessionToken.value}" } };
            }
            catch (BffHttpException ex)
            {
                throw new VCenterHttpException(ex);
            }
        }

        protected void Logout()
        {
            try
            {
                base.Delete("rest/com/vmware/cis/session",
                            null);
            }
            catch (BffHttpException ex)
            {
                throw new VCenterHttpException(ex);
            }
        }

        public override BffResponse Delete(
            string path,
            dynamic content)
        {
            if (!_initialized)
            {
                Initialize();
            }
            try
            {
                return base.Delete(path,
                                   (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new VCenterHttpException(ex);
            }
        }

        public override BffResponse Get(string path)
        {
            if (!_initialized)
            {
                Initialize();
            }
            try
            {
                return base.Get(path);
            }
            catch (BffHttpException ex)
            {
                throw new VCenterHttpException(ex);
            }
        }

        public override BffResponse Get(
            string path,
            Dictionary<string, dynamic> queryParams)
        {
            if (!_initialized)
            {
                Initialize();
            }
            try
            {
                return base.Get(path,
                                queryParams);
            }
            catch (BffHttpException ex)
            {
                throw new VCenterHttpException(ex);
            }
        }

        public override BffResponse Post(
            string path,
            dynamic content)
        {
            if (!_initialized)
            {
                Initialize();
            }
            try
            {
                return base.Post(path,
                                 (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new VCenterHttpException(ex);
            }
        }

        public override BffResponse Post(
            string path,
            Dictionary<string, dynamic> queryParams,
            dynamic content)
        {
            try
            {
                return base.Post(path, queryParams, (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new VCenterHttpException(ex);
            }
        }

        public override BffResponse Put(
            string path,
            dynamic content)
        {
            if (!_initialized)
            {
                Initialize();
            }
            try
            {
                return base.Put(path,
                                 (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new VCenterHttpException(ex);
            }
        }
    }
}
