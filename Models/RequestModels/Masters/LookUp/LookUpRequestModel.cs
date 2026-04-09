using Models.RequestModels;
using Models.RequestModels.Masters;
using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels.Masters.LookUp
{
    public class LookUpRequestModel : CommonRequestModel
    {
        public Guid TypeId { get; set; } // Foreign Key
        public string? Type { get; set; } // Type Field
        public string? Value { get; set; } // Value Field
        public string? Description { get; set; } // Description
        public string? URL { get; set; } // Optional URL Field
        public string? ActionBy { get; set; }

    }
}
