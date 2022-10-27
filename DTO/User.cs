namespace vmProjectBFF.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string CanvasToken { get; set; }
        public bool EmailIsVerified { get; set; }
        public int VerificationCode { get; set; }
        public DateTime VerificationCodeExpiration { get; set; }

    }
}