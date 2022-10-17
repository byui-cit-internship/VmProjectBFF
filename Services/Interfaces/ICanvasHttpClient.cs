using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public interface ICanvasHttpClient : IBffHttpClient
    {
        public void SetCanvasToken(string canvasToken);
    }
}
