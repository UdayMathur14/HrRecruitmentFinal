namespace Models.RequestModels.Common.Attachments
{
    public class AttachmentUpdateRequestModel
    {
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? Status { get; set; }
        public Guid? ActionBy { get; set; }
    }
}
