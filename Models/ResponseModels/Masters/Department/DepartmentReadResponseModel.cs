using Models.ResponseModels.Masters.Job;

namespace Models.ResponseModels.Masters.Department
{
    public class DepartmentReadResponseModel
    {
        public Guid id { get; set; }
        public string DeptName { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DeptMemberReadResponseModel? Owner { get; set; }
        public int? JobCount { get; set; }
        public List<DeptMemberReadResponseModel> DepartmentMembers { get; set; } = new();
        public List<JobReadResponseModel> Jobs { get; set; } = new();
    }
}
