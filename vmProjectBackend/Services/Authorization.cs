using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using vmProjectBackend.DAL;
using vmProjectBackend.DTO;
using vmProjectBackend.Models;

namespace vmProjectBackend.Services
{
    public class Authorization
    {
        private readonly Backend _backend;
        private readonly DatabaseContext _context;
        private readonly ILogger _logger;

        private readonly List<string> authTypes = new() { "professor", "admin", "user" };
        public Authorization(
            Backend backend,
            DatabaseContext context,
            ILogger logger)
        {
            _backend = backend;
            _context = context;
            _logger = logger;
        }


        /****************************************
        Given an email returns either a professor user or null if the email doesn't belong to a professor
        ****************************************/
        public User getAuth(string authType, int? sectionId = null)
        {
            try
            {
                if (authTypes.Contains(authType))
                {
                    if (authType == "professor") {
                        BackendResponse professorResponse = _backend.Get($"api/v2/authorization", new { authType = "professor", sectionId = sectionId });
                        return JsonConvert.DeserializeObject<User>(professorResponse.Response);
                    } else
                    {
                        BackendResponse otherResponse = _backend.Get($"api/v2/authorization", new {authType=authType});
                        return JsonConvert.DeserializeObject<User>(otherResponse.Response);
                    }
                    
                }
                else
                {
                    _logger.LogError($"Attempted to authenticate a user with an incorrect authType. Attempted to authenticate using \"{authType}\".");
                    return null;
                }
            }
            catch (BackendException be)
            {
                return null;
            }
        }
    }
}
