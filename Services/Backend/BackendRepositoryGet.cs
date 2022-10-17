using vmProjectBFF.DTO;
using Newtonsoft.Json;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public dynamic getInstancesByUser(int userId){
            _lastResponse = _backendHttpClient.Get("api/v2/VmInstance", new{userId = userId});
            List<VmInstance> vmInstances =  JsonConvert.DeserializeObject<List<VmInstance>>(_lastResponse.Response); 
            List<int> vmTemplateIds = (from instance in vmInstances
                                    select instance.VmTemplateId).Distinct().ToList();
            List<VmTemplate> vmTemplates = new();
            foreach (int vmTemplateId in vmTemplateIds)
            {
                _lastResponse = _backendHttpClient.Get("api/v2/VmTemplate", new{vmTemplateId = vmTemplateId});
                vmTemplates.Add(JsonConvert.DeserializeObject<VmTemplate>(_lastResponse.Response));  
            }
            List<dynamic> courses = new();
            foreach (int vmTemplateId in vmTemplateIds)
            {
                _lastResponse = _backendHttpClient.Get("api/v2/Course", new{vmTemplateId = vmTemplateId});
                dynamic course = JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response);
                course.VmTemplateId = vmTemplateId;
                courses.Add(course);
            }
            return (
                (from vi in vmInstances 
                join vt in vmTemplates
                on vi.VmTemplateId equals vt.VmTemplateId
                join c in courses 
                on vt.VmTemplateId equals c.VmTemplateId
                select new {
                    CourseCode = c.CourseCode,
                    VmTemplateName = vt.VmTemplateName,
                    VmInstanceExpireDate = vi.VmInstanceExpireDate
                }).ToList()
            );
        }
    }
}
