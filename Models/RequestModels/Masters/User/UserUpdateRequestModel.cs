namespace Models.RequestModels.Masters.User
{
    public class UserUpdateRequestModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public Guid? ActionBy { get; set; }
    }
}
