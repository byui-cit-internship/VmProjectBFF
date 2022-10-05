﻿using Newtonsoft.Json;
using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public class Authorization
    {
        private readonly BackendHttpClient _backend;
        private readonly ILogger _logger;

        private readonly List<string> authTypes = new() { "professor", "admin", "user" };
        public Authorization(
            BackendHttpClient backend,
            ILogger logger)
        {
            _backend = backend;
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
                    if (authType == "professor")
                    {
                        BffResponse professorResponse = _backend.Get($"api/v2/authorization", new { authType = "professor", sectionId = sectionId });
                        return JsonConvert.DeserializeObject<User>(professorResponse.Response);
                    }
                    else
                    {
                        BffResponse otherResponse = _backend.Get($"api/v2/authorization", new { authType = authType });
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
