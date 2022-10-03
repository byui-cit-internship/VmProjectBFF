using System.Net;

namespace vmProjectBFF.Exceptions
{
    public class VsphereHttpException : BffHttpException
    {
        public VsphereHttpException(
            HttpStatusCode statusCode, 
            string message) 
            : base(statusCode, message)
        {
        }
    }
}
