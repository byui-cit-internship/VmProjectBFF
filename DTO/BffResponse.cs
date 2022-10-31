using System.Net;

namespace VmProjectBFF.DTO
{
    public class BffResponse
    {
        public HttpStatusCode StatusCode { get; }
        public string Response { get; }
        public HttpResponseMessage HttpResponse { get; }

        public BffResponse(HttpResponseMessage httpResponse)
        {
            StatusCode = httpResponse.StatusCode;
            Response = httpResponse.Content.ReadAsStringAsync().Result;
            HttpResponse = httpResponse;
        }
    }
}
