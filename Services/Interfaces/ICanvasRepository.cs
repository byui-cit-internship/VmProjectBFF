using VmProjectBFF.DTO.Database;

namespace VmProjectBFF.Services
{
    public partial interface ICanvasRepository
    {
        public ICanvasHttpClient CanvasHttpClient { get; }

        public dynamic GetCoursesByCanvasToken(string canvasToken);
        public List<Semester> GetEnrollmentTerms(string canvasToken);
    }
}
