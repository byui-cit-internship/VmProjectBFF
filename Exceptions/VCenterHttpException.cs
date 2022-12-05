using System.Net;

namespace VmProjectBFF.Exceptions
{
    public class VCenterHttpException : BffHttpException
    {
        public VCenterHttpException(
            HttpStatusCode statusCode,
            string message)
            : base(
                  statusCode,
                  message)
        {
        }

        public VCenterHttpException(BffHttpException be)
        : this(
              be.StatusCode,
              be.Message)
        {
        }
    }
}
