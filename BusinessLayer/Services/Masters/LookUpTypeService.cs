using AutoMapper;
using BusinessLogic.Interfaces.Masters;
using DataAccess.Domain.Masters.LookUpType;
using DataAccess.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Models;
using Models.RequestModels.Masters.LookUpType;
using Models.ResponseModels.Masters.LookUpType;

namespace BusinessLogic.Services.Masters
{
    public class LookUpTypeService(ILookupTypeReporsitory lookupTypeReporsitory, IMapper mapper) : ILookUpTypeService
    {
        public async Task<LookUpTypeCreateResponseModel?> CreateLookUpAsync(LookUpTypeRequestModel requestModel)
        {
            LookUpTypeCreateResponseModel responseModel = new LookUpTypeCreateResponseModel();

            try
            {
                LookupTypeMstEntity? entity = await lookupTypeReporsitory.FindByTypeAsync(requestModel);

                if (entity != null)
                {
                    responseModel.responseCode = StatusCodes.Status400BadRequest;
                    responseModel.message = "Type Already Exist!";

                    return responseModel;
                }
                else
                {
                    LookupTypeMstEntity lookup = mapper.Map<LookupTypeMstEntity>(requestModel);
                    lookup.Status = "Active";
                    lookup.CreatedBy = lookup.ModifiedBy = requestModel.ActionBy;
                    lookup.CreatedOn = lookup.ModifiedOn = DateTime.UtcNow;

                    var result = await lookupTypeReporsitory.AddAsync(lookup);

                    responseModel.responseCode = 200;
                    responseModel.message = "Type Created Successfully!";
                    responseModel.Id = result;
                }
            }
            catch (Exception ex)
            {
                responseModel.responseCode = StatusCodes.Status400BadRequest;
                responseModel.message += ex.Message;
            }

            return responseModel;
        }

        public async Task<LookUpTypeReadResponseModel> GetLookUpTypeAsync(Guid id)
        {
            LookupTypeMstEntity? entity = await lookupTypeReporsitory.FindById(id);
            if (entity == null)
            {
                return null;
            }
            LookUpTypeReadResponseModel response = mapper.Map<LookUpTypeReadResponseModel>(entity);

            return response;
        }

        public async Task<LookUpTypeSearchResponse?> SearchLookUpAsync(LookUpTypeSearchRequestModel requestModel, string? offset, string count)
        {
            LookupTypeSearchResponseEntity entityResponse = await lookupTypeReporsitory.SearLookupTypeAsync(requestModel, offset, count);
            LookUpTypeSearchResponse userReadResponse = mapper.Map<LookUpTypeSearchResponse>(entityResponse);

            return userReadResponse;
        }

        public async Task<CommonResponseModel?> UpdateLookUpTypeAsync(LookUpTypeRequestModel requestModel, Guid id)
        {
            CommonResponseModel responseModel = new CommonResponseModel();
            LookupTypeMstEntity? entity = await lookupTypeReporsitory.FindById(id);

            if (entity != null)
            {
                entity.Description = requestModel.Description;
                entity.Status = requestModel.Status;
                entity.ModifiedBy = requestModel.ActionBy;
                entity.ModifiedOn = DateTime.Now;

                var guid = await lookupTypeReporsitory.UpdateAsync(entity);

                responseModel.responseCode = StatusCodes.Status200OK;
                responseModel.message = "Updated Successfully!";
            }
            else
            {
                responseModel.responseCode = StatusCodes.Status400BadRequest;
                responseModel.message = "Data Not Found!";
            }

            return responseModel;
        }
    }
}
