using Newtonsoft.Json;
using VmProjectBFF.DTO.Canvas;
using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.Services
{
    public partial class CanvasRepository
    {
        /****************************************
        Given an email returns either a professor user or null if the email doesn't belong to a professor
        ****************************************/
        public dynamic GetCoursesByCanvasToken(string canvasToken)
        {
            _canvasHttpClient.SetCanvasToken(canvasToken);
            _lastResponse = _canvasHttpClient.Get("api/v1/courses?per_page=1000");
            return JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response); 
        }

        public List<Semester> GetEnrollmentTerms(string canvasToken)
        {
            _canvasHttpClient.SetCanvasToken(canvasToken);
            _lastResponse = _canvasHttpClient.Get("/api/v1/accounts/1/terms?per_page=1000");
            List<EnrollmentTerm> termlist = JsonConvert.DeserializeObject<EnrollmentTermContainer>(_lastResponse.Response).enrollment_terms;
            termlist = 
                (from tl in termlist
                where tl.start_at is not null
                where tl.end_at is not null
                select tl).ToList();
            List<Semester> semesterList = new();
            foreach (EnrollmentTerm term in termlist) {
                semesterList.Add(new(term));
            }
            return semesterList;
        }

        public dynamic GetUserByCanvasToken(string canvasToken)
        {
            _canvasHttpClient.SetCanvasToken(canvasToken);
            _lastResponse = _canvasHttpClient.Get("/api/v1/users/self/profile");
            CanvasUser user = JsonConvert.DeserializeObject<CanvasUser>(_lastResponse.Response);
            
            return user;
        }
    }
}
