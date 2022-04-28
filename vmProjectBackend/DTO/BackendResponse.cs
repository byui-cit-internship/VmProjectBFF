using System.Net;
using System.Net.Http;

namespace vmProjectBackend.DTO
{
    public class BackendResponse
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _response;
        private readonly HttpResponseMessage _httpResponse;

        public HttpStatusCode StatusCode { get { return _statusCode; } }
        public string Response { get { return _response; } }
        public HttpResponseMessage HttpResponse { get { return _httpResponse; } }

        public BackendResponse(HttpStatusCode statusCode, string response, HttpResponseMessage httpResponse)
        {
            _statusCode = statusCode;
            _response = response;
            _httpResponse = httpResponse;
        }
    }
}
