using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using VmProjectBFF.DTO;
using VmProjectBFF.DTO.Database;
using VmProjectBFF.Exceptions;

namespace VmProjectBFF.Services
{
    public class BackgroundService1Manual : BackgroundService1, IBackgroundService1Manual
    {
        public BackgroundService1Manual (
            IBackendHttpClient backendHttpClient,
            ILogger<BackgroundService1> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor)
            : base(
            backendHttpClient,
            logger,
            httpClientFactory,
            configuration,
            contextAccessor)
            {
                
            }

    }
} 