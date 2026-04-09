using AutoMapper;
using DataAccessLayer.Domain.Masters.DepartmentSummary;
using Models.ResponseModels.Masters.DepartmentSummary;

namespace BusinessLayer.Mappings.Masters
{
    public class DepartmentSummaryMappingProfile : Profile
    {
        public DepartmentSummaryMappingProfile()
        {
            CreateMap<JobStatusSummaryItemEntity, JobStatusSummaryItemResponseModel>();
            CreateMap<DepartmentSummaryResponseEntity, DepartmentSummaryResponseModel>();
        }
    }
}
