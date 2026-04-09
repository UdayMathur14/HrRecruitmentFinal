using AutoMapper;
using BusinessLogic.Interfaces.Masters;
using DataAccess.Domain.Masters.LookUpMst;
using DataAccess.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Models;
using Models.RequestModels.Masters.LookUp;
using Models.ResponseModels.Masters.LookUp;

namespace BusinessLogic.Services.Masters
{
    public class LookupService(ILookupReporsitory lookupReporsitory, IMapper mapper) : ILookUpService
    {
        public async Task<LookUpCreateResponseModel?> CreateLookUpAsync(LookUpRequestModel requestModel)
        {
            LookUpCreateResponseModel responseModel = new LookUpCreateResponseModel();

            try
            {
                LookupMstEntity? lookUpEntity = await lookupReporsitory.FindByValue(requestModel);

                if (lookUpEntity != null)
                {
                    responseModel.responseCode = StatusCodes.Status400BadRequest;
                    responseModel.message = "Data Already Exist!";

                    return responseModel;
                }
                else
                {
                    LookupMstEntity entity = mapper.Map<LookupMstEntity>(requestModel);
                    entity.Status = "Active";
                    entity.CreatedOn = entity.ModifiedOn = DateTime.Now;
                    entity.CreatedBy = entity.ModifiedBy = requestModel.ActionBy;

                    var result = await lookupReporsitory.AddAsync(entity);

                    responseModel.responseCode = 200;
                    responseModel.message = "Created Successfully!";
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

        public async Task<LookUpReadResponseModel> GetLookUpAsync(Guid id)
        {
            LookupMstEntity? entity = await lookupReporsitory.FindAsync(id);
            if (entity == null)
            {
                return null;
            }
            LookUpReadResponseModel response = mapper.Map<LookUpReadResponseModel>(entity);

            return response;
        }

        public async Task<LookUpSearchResponse> SearchLookUpAsync(LookUpSearchRequestModel requestModel, string loginUserId, string? offset, string count)
        {
            LookupSearchResponseEntity entityResponse = await lookupReporsitory.SearchLookUpAsync(requestModel, offset, count);
            LookUpSearchResponse lookupReadResponse = mapper.Map<LookUpSearchResponse>(entityResponse);

            return lookupReadResponse;
        }

        public async Task<CommonResponseModel> UpdateLookUpAsync(LookUpRequestModel requestModel, Guid id)
        {
            CommonResponseModel responseModel = new CommonResponseModel();
            LookupMstEntity? entity = await lookupReporsitory.FindAsync(id);

            if (entity != null)
            {
                entity.Description = requestModel.Description;
                entity.URL = requestModel.URL;
                entity.Status = requestModel.Status;
                entity.ModifiedBy = requestModel.ActionBy;
                entity.ModifiedOn = DateTime.Now;

                var guid = await lookupReporsitory.UpdateAsync(entity);

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
