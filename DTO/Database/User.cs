namespace VmProjectBFF.DTO.Database
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public string CanvasToken { get; set; }

        public string role { get; set; }

        public string approveStatus { get; set; }
    }
}