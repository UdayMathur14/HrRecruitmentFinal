namespace Models.ResponseModels.Masters.User
{
    public class UserReadResponseModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
