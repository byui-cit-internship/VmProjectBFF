using VmProjectBFF.DTO;

namespace VmProjectBFF.Services
{
    public interface ICanvasHttpClient : IBffHttpClient
    {
        public void SetCanvasToken(string canvasToken);
    }
}
