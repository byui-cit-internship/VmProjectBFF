using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Services
{
    public class CanvasHttpClient : BffHttpClient, ICanvasHttpClient
    {
        protected static string GetBaseUrl(IConfiguration configuration)
        {
            return configuration.GetConnectionString("CanvasRootUri");
        }

        public CanvasHttpClient(
            IConfiguration configuration,
            ILogger<CanvasHttpClient> logger)
            : base(
                  GetBaseUrl(configuration),
                  null,
                  logger)
        {
        }

        public void SetCanvasToken(string canvasToken)
        {
            Headers = new() { { "Authorization", $"Bearer {canvasToken}" } };
        }

        public override BffResponse Delete(
            string path,
            dynamic content)
        {
            try
            {
                return base.Delete(path,
                                   (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new CanvasHttpException(ex);
            }
        }

        public override BffResponse Get(string path)
        {
            try
            {
                return base.Get(path);
            }
            catch (BffHttpException ex)
            {
                throw new CanvasHttpException(ex);
            }
        }

        public override BffResponse Get(
            string path,
            Dictionary<string, dynamic> queryParams)
        {
            try
            {
                return base.Get(path,
                                queryParams);
            }
            catch (BffHttpException ex)
            {
                throw new CanvasHttpException(ex);
            }
        }

        public override BffResponse Post(
            string path,
            dynamic content)
        {
            try
            {
                return base.Post(path,
                                 (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new CanvasHttpException(ex);
            }
        }

        public override BffResponse Put(
            string path,
            dynamic content)
        {
            try
            {
                return base.Put(path,
                                (object)content);
            }
            catch (BffHttpException ex)
            {
                throw new CanvasHttpException(ex);
            }
        }
    }
}
