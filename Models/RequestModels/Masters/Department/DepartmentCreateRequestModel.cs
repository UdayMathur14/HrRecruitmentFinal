namespace Models.RequestModels.Masters.Department
{
    public class DepartmentCreateRequestModel
    {
        public string DeptName { get; set; } 
        public string? Location { get; set; }
        public string? Description { get; set; }
        public Guid? OwnerId { get; set; }
        public List<DeptMemberRequestModel> DepartmentMembers { get; set; } = new();
        public Guid? CreatedBy { get; set; }
    }

    public class DeptMemberRequestModel
    {
        public Guid UserId { get; set; }
    }
}
