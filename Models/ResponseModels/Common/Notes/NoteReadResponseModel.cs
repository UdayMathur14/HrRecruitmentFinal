namespace Models.ResponseModels.Common.Notes
{
    public class NoteReadResponseModel
    {
        public Guid Id { get; set; }
        public int ReferenceType { get; set; }
        public Guid ReferenceId { get; set; }
        public string Header { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string? Status { get; set; }
    }
}
