using Newtonsoft.Json;

namespace vmProjectBFF.Services
{
    public partial class CanvasRepository
    {
        /****************************************
        Given an email returns either a professor user or null if the email doesn't belong to a professor
        ****************************************/
        public dynamic GetCourses(string canvasToken)
        {
            _canvasHttpClient.SetCanvasToken(canvasToken);
            _lastResponse = _canvasHttpClient.Get("api/v1/courses");
            return JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response); 
        }
    }
}
