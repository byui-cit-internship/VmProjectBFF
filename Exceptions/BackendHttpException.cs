using System.Net;

namespace vmProjectBFF.Exceptions
{
    public class BackendHttpException : BffHttpException
    {

        public BackendHttpException(
            HttpStatusCode statusCode,
            string message)
            : base(
                  statusCode,
                  message)
        {
        }

        public BackendHttpException(BffHttpException be)
        : this(
              be.StatusCode,
              be.Message)
        {
        }
    }
}
