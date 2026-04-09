using System.Text.Json.Serialization;

namespace Models.ResponseModels.BaseResponseSetup
{
    public abstract class SearchResponseBase<TEntity> : CommonResponseModel
    {
        [JsonIgnore]
        public List<TEntity> Results { get; set; } = new List<TEntity>(); 
        public PagingModel Paging { get; set; } = new PagingModel();
        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();

    }
}
