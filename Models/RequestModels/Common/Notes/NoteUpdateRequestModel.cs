namespace Models.RequestModels.Common.Notes
{
    public class NoteUpdateRequestModel
    {
        public string? Header { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public Guid? ActionBy { get; set; }
    }
}
