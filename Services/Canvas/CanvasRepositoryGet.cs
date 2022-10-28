using Newtonsoft.Json;
using vmProjectBFF.DTO;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public partial class CanvasRepository
    {
        /****************************************
        Given an email returns either a professor user or null if the email doesn't belong to a professor
        ****************************************/
        public dynamic GetCoursesByCanvasToken(string canvasToken)
        {
            _canvasHttpClient.SetCanvasToken(canvasToken);
            _lastResponse = _canvasHttpClient.Get("api/v1/courses");
            return JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response); 
        }
        public List<Semester> GetEnrollmentTerms(string canvasToken)
        {
            _canvasHttpClient.SetCanvasToken(canvasToken);
            _lastResponse = _canvasHttpClient.Get("/api/v1/accounts/1/terms?per_page=1000");
            List<Test1> termlist = JsonConvert.DeserializeObject<EnrollmentTerm>(_lastResponse.Response).enrollment_terms;
            termlist = 
                (from tl in termlist
                where tl.start_at is not null
                where tl.end_at is not null
                select tl).ToList();
            List<Semester> semesterList = new();
            foreach (Test1 term in termlist) {
                semesterList.Add(new(term));
            }
            return semesterList;
        }
    }
}
