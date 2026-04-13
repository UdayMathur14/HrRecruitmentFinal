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
                var entity = await departmentRepository.FindAsync(id);
                if (entity == null)
                {
                    responseModel.responseCode = 400;
                    responseModel.message = "Data Not Found!";
                    return responseModel;
                }

                entity.DeptName = requestModel.DeptName ?? entity.DeptName;
                entity.Location = requestModel.Location ?? entity.Location;
                entity.Description = requestModel.Description ?? entity.Description;
                entity.Status = requestModel.Status ?? entity.Status;
                entity.OwnerId = requestModel.OwnerId;
                entity.ModifiedOn = DateTime.Now;
                entity.ModifiedBy = requestModel.ActionBy;

                entity.OwnerUser = null;
                entity.DepartmentMembers = null;

                var newMembers = requestModel.DepartmentMembers.Select(m => new DepartmentMembersEntity
                {
                    Id = Guid.NewGuid(),
                    DeptId = id,
                    UserId = m.UserId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = requestModel.ActionBy
                }).ToList();

                await departmentRepository.ReplaceMembersAsync(id, newMembers);

                await departmentRepository.UpdateAsync(entity);

                responseModel.responseCode = 200;
                responseModel.message = "Updated Successfully!";
                responseModel.Id = entity.Id;
            }
            catch (Exception ex)
            {
                responseModel.responseCode = 400;
                responseModel.message = ex.InnerException?.Message ?? ex.Message;
            }
            return responseModel;
        }
    }
}
