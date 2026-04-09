namespace Models.RequestModels.Masters.LookUp
{
    public class LookUpSearchRequestModel:CommonRequestModel
    {
        public string? Type { get; set; }
        public string? Value { get; set; }
    }
}
