using System.Net;

namespace vmProjectBFF.Exceptions
{
    public class VCenterHttpException : BffHttpException
    {
        public VCenterHttpException(
            HttpStatusCode statusCode, 
            string message) 
            : base(statusCode, message)
        {
        }
    }
}
