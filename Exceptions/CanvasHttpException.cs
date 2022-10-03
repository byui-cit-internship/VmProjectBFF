using System.Net;

namespace vmProjectBFF.Exceptions
{
    public class CanvasHttpException : BffHttpException
    {
        public CanvasHttpException(
            HttpStatusCode statusCode, 
            string message) 
            : base(statusCode, message)
        {
        }
    }
}
