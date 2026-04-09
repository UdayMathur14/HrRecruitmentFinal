using AutoMapper;
using BusinessLayer.Interfaces.Masters;
using DataAccessLayer.Domain.Masters.User;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Models;
using Models.RequestModels.Masters.User;
using Models.ResponseModels.Masters.User;

namespace BusinessLayer.Services.Masters
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        public async Task<UserReadResponseModel?> GetByIdAsync(Guid id)
        {
            UserEntity? entity = await userRepository.FindAsync(id);

            if (entity == null)
                return null;

            UserReadResponseModel response = mapper.Map<UserReadResponseModel>(entity);
            return response;
        }

        public async Task<CommonResponseModel> CreateUserAsync(UserCreateRequestModel requestModel)
        {
            var response = new CommonResponseModel();

            try
            {
                var entity = mapper.Map<UserEntity>(requestModel);
                entity.CreatedOn = DateTime.Now;
                entity.Status = "Active";

                var result = await userRepository.AddAsync(entity);

                response.responseCode = StatusCodes.Status200OK;
                response.message = "Successfully Created";
                response.Id = result;
            }
            catch (Exception ex)
            {
                response.responseCode = StatusCodes.Status400BadRequest;
                response.message = ex.Message;
            }

            return response;
        }

        public async Task<UserSearchResponseModel?> SearchUserAsync(UserSearchRequestModel requestModel, string? offset, string count)
        {
            UserSearchResponseEntity entityResponse = await userRepository.SearchUserAsync(requestModel, offset, count);
            UserSearchResponseModel response = mapper.Map<UserSearchResponseModel>(entityResponse);

            return response;
        }

        public async Task<CommonResponseModel> UpdateUserAsync(Guid id, UserUpdateRequestModel requestModel)
        {
            CommonResponseModel responseModel = new CommonResponseModel();

            try
            {
                UserEntity? entity = await userRepository.FindAsync(id);

                if (entity == null)
                {
                    responseModel.responseCode = StatusCodes.Status400BadRequest;
                    responseModel.message = "Data Not Found!";
                    return responseModel;
                }

                if (!string.IsNullOrWhiteSpace(requestModel.FirstName))
                    entity.FirstName = requestModel.FirstName;

                if (!string.IsNullOrWhiteSpace(requestModel.LastName))
                    entity.LastName = requestModel.LastName;

                if (!string.IsNullOrWhiteSpace(requestModel.Email))
                    entity.Email = requestModel.Email;

                if (!string.IsNullOrWhiteSpace(requestModel.PhoneNumber))
                    entity.PhoneNumber = requestModel.PhoneNumber;

                if (!string.IsNullOrWhiteSpace(requestModel.PasswordHash))
                    entity.PasswordHash = requestModel.PasswordHash;

                if (!string.IsNullOrWhiteSpace(requestModel.Role))
                    entity.Role = requestModel.Role;

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                    entity.Status = requestModel.Status;

                entity.ModifiedOn = DateTime.Now;
                entity.ModifiedBy = requestModel.ActionBy;

                await userRepository.UpdateAsync(entity);

                responseModel.responseCode = StatusCodes.Status200OK;
                responseModel.message = "Updated Successfully!";
                responseModel.Id = entity.Id;
            }
            catch (Exception ex)
            {
                responseModel.responseCode = StatusCodes.Status400BadRequest;
                responseModel.message = ex.Message;
            }

            return responseModel;
        }
    }
}
