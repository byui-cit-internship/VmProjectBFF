namespace vmProjectBFF.Services
{
    public partial interface ICanvasRepository
    {
        public ICanvasHttpClient CanvasHttpClient { get; }

        public dynamic GetCoursesByCanvasToken(string canvasToken);
    }
}
