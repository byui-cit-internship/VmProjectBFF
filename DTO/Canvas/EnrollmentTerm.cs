namespace VmProjectBFF.DTO.Canvas
{
    public struct EnrollmentTerm
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? start_at { get; set; }
        public DateTime? end_at { get; set; }
    }
}