using System.Net;

namespace vmProjectBFF.Exceptions
{
    public class CanvasHttpException : BffHttpException
    {
        public CanvasHttpException(
            HttpStatusCode statusCode,
            string message)
            : base(
                  statusCode,
                  message)
        {
        }

        public CanvasHttpException(BffHttpException be)
        : this(
              be.StatusCode,
              be.Message)
        {
        }
    }
}
