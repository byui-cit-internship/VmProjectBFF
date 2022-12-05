using Newtonsoft.Json;
using VmProjectBFF.DTO;
using VmProjectBFF.Exceptions;
using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.Services
{
    public class Authorization : IAuthorization
    {
        private readonly IBackendHttpClient _backendHttpClient;
        private readonly ILogger<Authorization> _logger;

        private readonly List<string> authTypes = new() { "professor", "admin", "user" };
        public Authorization(
            IBackendHttpClient backendHttpClient,
            ILogger<Authorization> logger)
        {
            _backendHttpClient = backendHttpClient;
            _logger = logger;
        }


        /****************************************
        Given an email returns either a professor user or null if the email doesn't belong to a professor
        ****************************************/
        public User GetAuth(
            string authType,
            int? sectionId = null)
        {
            try
            {
                if (authTypes.Contains(authType))
                {
                    if (authType == "professor")
                    {
                        BffResponse professorResponse = _backendHttpClient.Get($"api/v2/authorization", new() { { "authType", "professor" }, { "sectionId", sectionId } });
                        return JsonConvert.DeserializeObject<User>(professorResponse.Response);
                    }
                    else
                    {
                        BffResponse otherResponse = _backendHttpClient.Get($"api/v2/authorization", new() { { "authType", authType } });
                        return JsonConvert.DeserializeObject<User>(otherResponse.Response);
                    }
                }
                else
                {
                    _logger.LogError($"Attempted to authenticate a user with an incorrect authType. Attempted to authenticate using \"{authType}\".");
                    return null;
                }
            }
            catch (BffHttpException be)
            {
                return null;
            }
        }
    }
}
