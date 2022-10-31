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
            _lastResponse = _canvasHttpClient.Get("api/v1/courses");
            return JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response); 
        }
<<<<<<< HEAD
=======

>>>>>>> main
        public List<Semester> GetEnrollmentTerms(string canvasToken)
        {
            _canvasHttpClient.SetCanvasToken(canvasToken);
            _lastResponse = _canvasHttpClient.Get("/api/v1/accounts/1/terms?per_page=1000");
<<<<<<< HEAD
            List<Test1> termlist = JsonConvert.DeserializeObject<EnrollmentTerm>(_lastResponse.Response).enrollment_terms;
=======
            List<EnrollmentTerm> termlist = JsonConvert.DeserializeObject<EnrollmentTermContainer>(_lastResponse.Response).enrollment_terms;
>>>>>>> main
            termlist = 
                (from tl in termlist
                where tl.start_at is not null
                where tl.end_at is not null
                select tl).ToList();
            List<Semester> semesterList = new();
<<<<<<< HEAD
            foreach (Test1 term in termlist) {
=======
            foreach (EnrollmentTerm term in termlist) {
>>>>>>> main
                semesterList.Add(new(term));
            }
            return semesterList;
        }
    }
}
