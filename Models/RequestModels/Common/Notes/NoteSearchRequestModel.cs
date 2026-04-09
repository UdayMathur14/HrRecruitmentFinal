using Models.Enums;

namespace Models.RequestModels.Common.Notes
{
    public class NoteSearchRequestModel
    {
        public ReferenceType? ReferenceType { get; set; }
        public Guid? ReferenceId { get; set; }
        public string? Header { get; set; }
        public string? Status { get; set; }
    }
}
