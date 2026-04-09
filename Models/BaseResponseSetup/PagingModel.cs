namespace Models.ResponseModels.BaseResponseSetup
{
    public class PagingModel
    {
        public int CurrentPage { get; set; }
        public int Results { get; set; }
        public long Total { get; set; }
        public int TotalPages { get; set; }
        public string? NextOffset { get; set; }
        public string? NextPage { get; set; }
        public string? PrevPage { get; set; }

    }
}
