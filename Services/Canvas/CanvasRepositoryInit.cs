using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using vmProjectBFF.DTO;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public partial class CanvasRepository : ICanvasRepository
    {
        protected readonly ILogger<CanvasRepository> _logger;
        protected BffResponse _lastResponse;

        public readonly ICanvasHttpClient _canvasHttpClient;
        public ICanvasHttpClient CanvasHttpClient { get { return _canvasHttpClient; } }

        public CanvasRepository(
            ILogger<CanvasRepository> logger,
            ICanvasHttpClient canvasHttpClient)
        {
            _logger = logger;
            _canvasHttpClient = canvasHttpClient;
        }
    }
}
