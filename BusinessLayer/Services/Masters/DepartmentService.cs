using AutoMapper;
using BusinessLayer.Interfaces.Masters;
using DataAccessLayer.Domain.Masters.Department;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Models;
using Models.RequestModels.Masters.Department;
using Models.ResponseModels.Masters.Department;

namespace BusinessLayer.Services.Masters
{
    public class DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper) : IDepartmentService
    {
        public async Task<List<DepartmentReadResponseModel>> GetAllAsync()
        {
            List<DepartmentEntity> entities = await departmentRepository.GetAllWithMembersAsync();
            return mapper.Map<List<DepartmentReadResponseModel>>(entities);
        }

        public async Task<DepartmentReadResponseModel?> GetByIdAsync(Guid id)
        {
            DepartmentEntity? entity = await departmentRepository.FindAsync(id);

            if (entity == null)
                return null;

            return mapper.Map<DepartmentReadResponseModel>(entity);
        }

        public async Task<CommonResponseModel> CreateFullDepartmentAsync(DepartmentCreateRequestModel DeptModel)
        {
            var response = new CommonResponseModel();

            try
            {
                var departmentEntity = mapper.Map<DepartmentEntity>(DeptModel);
                departmentEntity.CreatedOn = DateTime.Now;
                departmentEntity.Status = "Active";

                if (departmentEntity.DepartmentMembers != null)
                {
                    foreach (var member in departmentEntity.DepartmentMembers)
                    {
                        member.CreatedOn = DateTime.Now;
                        member.CreatedBy = DeptModel.CreatedBy;
                        member.Status = "Active";
                    }
                }

                var result = await departmentRepository.AddAsync(departmentEntity);

                response.responseCode = 200;
                response.message = "Successfully Created";
                response.Id = result;
            }
            catch (Exception ex)
            {
                response.responseCode = StatusCodes.Status400BadRequest;
                response.message += ex.Message;
            }

            return response;
        }

        public async Task<DeptSearchResponseModel?> SearchDeptAsync(DepartmentSearchRequestModel requestModel, string? offset, string count)
        {
            DepartmentSearchResponseEntity entityResponse = await departmentRepository.SearchDeptAsync(requestModel, offset, count);
            return mapper.Map<DeptSearchResponseModel>(entityResponse);
        }

        public async Task<CommonResponseModel> UpdateDepartmentAsync(Guid id, DepartmentUpdateRequestModel requestModel)
        {
            CommonResponseModel responseModel = new CommonResponseModel();

            try
            {
                DepartmentEntity? entity = await departmentRepository.FindAsync(id);

                if (entity == null)
                {
                    responseModel.responseCode = StatusCodes.Status400BadRequest;
                    responseModel.message = "Data Not Found!";
                    return responseModel;
                }

                if (!string.IsNullOrWhiteSpace(requestModel.DeptName))
                    entity.DeptName = requestModel.DeptName;

                if (!string.IsNullOrWhiteSpace(requestModel.Location))
                    entity.Location = requestModel.Location;

                if (!string.IsNullOrWhiteSpace(requestModel.Description))
                    entity.Description = requestModel.Description;

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                    entity.Status = requestModel.Status;

                if (requestModel.OwnerId.HasValue)
                    entity.OwnerId = requestModel.OwnerId;

                entity.ModifiedOn = DateTime.Now;
                entity.ModifiedBy = requestModel.ActionBy;

                entity.OwnerUser = null;
                entity.DepartmentMembers = null;

                if (requestModel.DepartmentMembers.Count > 0)
                {
                    var members = requestModel.DepartmentMembers
                        .Select(member => new DepartmentMembersEntity
                        {
                            DeptId = entity.Id,
                            UserId = member.UserId,
                            CreatedOn = DateTime.Now,
                            CreatedBy = requestModel.ActionBy,
                            Status = "Active"
                        })
                        .ToList();

                    await departmentRepository.ReplaceMembersAsync(entity.Id, members);
                }

                await departmentRepository.UpdateAsync(entity);

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
