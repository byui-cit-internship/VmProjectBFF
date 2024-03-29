﻿using VmProjectBFF.DTO;
using Newtonsoft.Json;
using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.Services
{
    public partial class BackendRepository
    {
        public dynamic GetSectionBySemester(int semester)
        {
            _lastResponse = _backendHttpClient.Get($"api/v1/section/sectionList", new() { { "semester", semester } });
            return JsonConvert.DeserializeObject<dynamic>(_lastResponse.Response);
        }

        public Section GetSectionById(int sectionId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "sectionId", sectionId } });
            return JsonConvert.DeserializeObject<Section>(_lastResponse.Response);
        }

        public List<Course> GetCoursesByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Course", new() { { "userId", userId } });
            return JsonConvert.DeserializeObject<List<Course>>(_lastResponse.Response);
        }

        public List<Section> GetSectionsByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "userId", userId } });
            return JsonConvert.DeserializeObject<List<Section>>(_lastResponse.Response);
        }

        public List<Section> GetSectionsByCourseId(int courseId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "courseId", courseId } });
            return JsonConvert.DeserializeObject<List<Section>>(_lastResponse.Response);
        }

        public List<Semester> GetAllSemesters(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Semester", new() { { "userId", userId } });
            return JsonConvert.DeserializeObject<List<Semester>>(_lastResponse.Response);
        }

        public Semester GetSemesterBySemesterId(int semesterId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Semester", new() { { "semesterId", semesterId } });
            return JsonConvert.DeserializeObject<Semester>(_lastResponse.Response);
        }

        public List<User> GetUsersBySection(int sectionId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/User", new() { { "sectionId", sectionId } });
            return JsonConvert.DeserializeObject<List<User>>(_lastResponse.Response);
        }

        public dynamic GetInstancesByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get("api/v2/VmInstance", new() { { "userId", userId } });
            List<VmInstance> vmInstances = JsonConvert.DeserializeObject<List<VmInstance>>(_lastResponse.Response);
            List<string> vmTemplateIds = (from instance in vmInstances
                                       select instance.VmTemplateId).Distinct().ToList();

            List<CourseTemplateDTO> courses = new();
            List<Section> sections = new();
            foreach (VmInstance vmInstance in vmInstances)
            {   
                VmProjectBFF.DTO.VCenter.VmTemplate vmTemplate= _vCenter.GetTemplateByVCenterId (vmInstance.VmTemplateId);

                vmInstance.vmTemplateName = vmTemplate.name;

                Console.WriteLine("section ID:"+vmInstance.SectionId);
                
                CourseTemplateDTO course = new CourseTemplateDTO();
                course.VmTemplateId = vmInstance.VmTemplateId;
                _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "sectionId", vmInstance.SectionId } });
                Section section = JsonConvert.DeserializeObject<Section>(_lastResponse.Response);
                sections.Add(section);
                course.CourseId = section.CourseId;
                course.CourseCode= section.CourseCode;
                courses.Add(course);
            
            }
            return (
                (from vi in vmInstances
                 join s in sections 
                 on vi.SectionId equals s.SectionId
                 join c in courses
                 on s.CourseId equals c.CourseId

                 select new
                 {
                     CourseCode = c.CourseCode,
                     VmInstanceVcenterId = vi.VmInstanceVcenterId,
                     VmInstanceCreationDate = vi.VmInstanceCreationDate,
                     VmInstanceExpireDate = vi.VmInstanceExpireDate,
                     VmInstanceVcenterName = vi.VmInstanceVcenterName,
                     SectionId = vi.SectionId,
                     vmTemplateName = vi.vmTemplateName
                 }).Distinct().ToList()
            );
        }

        public CreateVmDTO GetCreateVmByEnrollmentId(int enrollmentId)
        {
            _lastResponse = _backendHttpClient.Get("api/v1/CreateVm", new() { { "enrollmentId", enrollmentId } });
            return JsonConvert.DeserializeObject<List<CreateVmDTO>>(_lastResponse.Response).FirstOrDefault();
        }

        public Section GetSectionsByEnrollmentId(int enrollmentId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "userSectionRoleId", enrollmentId } });
            return JsonConvert.DeserializeObject<Section>(_lastResponse.Response);
        }

        public ResourcePool GetResourcePoolByResourcePoolId(int resourcePoolId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/ResourcePool", new() { { "resourcePoolId", resourcePoolId } });
            return JsonConvert.DeserializeObject<ResourcePool>(_lastResponse.Response);
        }

        public Folder GetFolderByFolderId(int folderId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Folder", new() { { "folderId", folderId } });
            return JsonConvert.DeserializeObject<Folder>(_lastResponse.Response);
        }

        public VmTemplate GetTemplateByVCenterId(string vCenterId)
        {
            _lastResponse = _backendHttpClient.Get("api/v2/VmTemplate", new() { { "vmTemplateVcenterId", vCenterId } });
            return JsonConvert.DeserializeObject<VmTemplate>(_lastResponse.Response);
        }

        public List<CourseListByUserDTO> GetStudentCourseByUserId(int userId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "userId", userId } });
            return JsonConvert.DeserializeObject<List<CourseListByUserDTO>>(_lastResponse.Response);
        }

        public User GetUserByEmail(string email)
        {
            _lastResponse = _backendHttpClient.Get("api/v2/User", new() { { "email", email } });
            return JsonConvert.DeserializeObject<User>(_lastResponse.Response);
        }

        public List<User> GetAdmins()
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/user", new() { { "isAdmin", true } });
            return JsonConvert.DeserializeObject<List<User>>(_lastResponse.Response);
        }
        public List<User> GetProfessors()
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/user", new() { { "role", "professor" } });
            return JsonConvert.DeserializeObject<List<User>>(_lastResponse.Response);
        }
        public List<VmTemplate> GetTemplatesByLibraryId(string libraryVCenterId)
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/VmTemplate", new() {{"libraryVCenterId", libraryVCenterId}});
            return JsonConvert.DeserializeObject<List<VmTemplate>>(_lastResponse.Response);
        }


    }
}