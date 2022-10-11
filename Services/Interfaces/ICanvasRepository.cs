namespace vmProjectBFF.Services
{
    public partial interface ICanvasRepository
    {
        public ICanvasHttpClient CanvasHttpClient { get; }

        public dynamic GetCourses(string canvasToken)
        {
            return null;
        }
    }
}
