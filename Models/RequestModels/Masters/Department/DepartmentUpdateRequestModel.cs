namespace Models.RequestModels.Masters.Department
{
    public class DepartmentUpdateRequestModel
    {
        public string? DeptName { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? ActionBy { get; set; }
        public List<DeptMemberRequestModel> DepartmentMembers { get; set; } = new();
    }
}
