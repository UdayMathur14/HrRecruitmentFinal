using AutoMapper;
using BusinessLayer.Interfaces.Masters;
using DataAccessLayer.Domain.Masters.Job;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Models;
using Models.RequestModels.Masters.Job;
using Models.ResponseModels.Masters.Job;

namespace BusinessLayer.Services.Masters
{
    public class JobService(IJobRepository jobRepository, IMapper mapper) : IJobService
    {
        public async Task<JobReadResponseModel?> GetByIdAsync(Guid id)
        {
            JobEntity? entity = await jobRepository.FindAsync(id);

            if (entity == null)
                return null;

            JobReadResponseModel response = mapper.Map<JobReadResponseModel>(entity);
            return response;
        }

        public async Task<CommonResponseModel> CreateJobAsync(JobCreateRequestModel requestModel)
        {
            var response = new CommonResponseModel();

            try
            {
                var entity = mapper.Map<JobEntity>(requestModel);
                entity.CreatedOn = DateTime.Now;
                entity.Status = "Active";

                if (entity.JobMembers != null)
                {
                    foreach (var member in entity.JobMembers)
                    {
                        member.CreatedOn = DateTime.Now;
                        member.CreatedBy = requestModel.CreatedBy;
                        member.Status = "Active";
                    }
                }

                var result = await jobRepository.AddAsync(entity);

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

        public async Task<JobSearchResponseModel?> SearchJobAsync(JobSearchRequestModel requestModel, string? offset, string count)
        {
            JobSearchResponseEntity entityResponse = await jobRepository.SearchJobAsync(requestModel, offset, count);
            JobSearchResponseModel response = mapper.Map<JobSearchResponseModel>(entityResponse);

            return response;
        }

        public async Task<CommonResponseModel> UpdateJobAsync(Guid id, JobUpdateRequestModel requestModel)
        {
            CommonResponseModel responseModel = new CommonResponseModel();

            try
            {
                JobEntity? entity = await jobRepository.FindAsync(id);

                if (entity == null)
                {
                    responseModel.responseCode = StatusCodes.Status400BadRequest;
                    responseModel.message = "Data Not Found!";
                    return responseModel;
                }

                if (requestModel.DeptId.HasValue)
                    entity.DeptId = requestModel.DeptId.Value;

                if (!string.IsNullOrWhiteSpace(requestModel.JobName))
                    entity.JobName = requestModel.JobName;

                if (!string.IsNullOrWhiteSpace(requestModel.Description))
                    entity.Description = requestModel.Description;

                if (requestModel.HeadCount.HasValue)
                    entity.HeadCount = requestModel.HeadCount.Value;

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                    entity.Status = requestModel.Status;

                if (!string.IsNullOrWhiteSpace(requestModel.JobStage))
                    entity.JobStage = requestModel.JobStage;

                if (requestModel.JobOwnerId.HasValue)
                    entity.JobOwnerId = requestModel.JobOwnerId;

                entity.ModifiedOn = DateTime.Now;
                entity.ModifiedBy = requestModel.ActionBy;

                // Clear navigational properties to avoid tracking conflicts
                entity.JobOwnerUser = null;
                entity.JobMembers = null;

                var members = requestModel.JobMembers
                    .Select(member => new JobMembersEntity
                    {
                        JobId = entity.Id,
                        UserId = member.UserId,
                        CreatedOn = DateTime.Now,
                        CreatedBy = requestModel.ActionBy,
                        Status = "Active"
                    })
                    .ToList();

                // Update members first, then the job
                await jobRepository.ReplaceMembersAsync(entity.Id, members);

                await jobRepository.UpdateAsync(entity);

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
