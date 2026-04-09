namespace Models.RequestModels.Masters.LookUpType
{
    public class LookUpTypeRequestModel : CommonRequestModel
    {
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? ActionBy { get; set; }
    }
}
