using Models.Enums;

namespace Models.RequestModels.Common.Notes
{
    public class NoteCreateRequestModel
    {
        public ReferenceType ReferenceType { get; set; }
        public Guid ReferenceId { get; set; }
        public string Header { get; set; }
        public string? Description { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
