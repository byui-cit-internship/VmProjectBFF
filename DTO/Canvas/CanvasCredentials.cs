// this model is not link to the Database, it is just used to recieve data from post methods [HttpPost("professor/checkCanvasToken")]

namespace VmProjectBFF.DTO.Canvas
{
    public class CanvasCredentials
    {
        public string canvas_token { get; set; }
        public long canvas_course_id { get; set; }
    }
}