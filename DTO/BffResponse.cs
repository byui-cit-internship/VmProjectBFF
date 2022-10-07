using System.Net;

namespace vmProjectBFF.DTO
{
    public class BffResponse
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _response;
        private readonly HttpResponseMessage _httpResponse;

        public HttpStatusCode StatusCode { get { return _statusCode; } }
        public string Response { get { return _response; } }
        public HttpResponseMessage HttpResponse { get { return _httpResponse; } }

        public BffResponse(HttpResponseMessage httpResponse)
        {
            _statusCode = httpResponse.StatusCode;
            _response = httpResponse.Content.ReadAsStringAsync().Result;
            _httpResponse = httpResponse;
        }
    }
}
