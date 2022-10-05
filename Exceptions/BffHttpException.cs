using System.Net;

namespace vmProjectBFF.Exceptions
{
    public class BffHttpException : Exception
    {
        protected readonly HttpStatusCode _statusCode;

        public HttpStatusCode StatusCode { get { return _statusCode; } }

        public BffHttpException(
            HttpStatusCode statusCode,
            string message)
            : base(message)
        {
            _statusCode = statusCode;
        }
    }
}