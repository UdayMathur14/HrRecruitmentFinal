using AutoMapper;
using BusinessLayer.Interfaces.Masters;
using DataAccessLayer.Domain.Masters.Candidate;
using DataAccessLayer.Interfaces.Common;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Models;
using Models.RequestModels.Masters.Candidate;
using Models.ResponseModels.Masters.Candidate;

namespace BusinessLayer.Services.Masters
{
    public class CandidateService(
        ICandidateRepository candidateRepository,
        IReferenceValidationRepository referenceValidationRepository,
        IMapper mapper) : ICandidateService
    {
        private static readonly HashSet<string> AllowedCandidateStatuses = new(StringComparer.OrdinalIgnoreCase)
        {
            "Applied", "Screening", "Interview", "Offer", "Hired", "Rejected", "OnHold"
        };

        public async Task<CandidateReadResponseModel?> GetByIdAsync(Guid id)
        {
            CandidateEntity? entity = await candidateRepository.FindAsync(id);
            if (entity == null)
                return null;

            return mapper.Map<CandidateReadResponseModel>(entity);
        }

        public async Task<CommonResponseModel> CreateCandidateAsync(CandidateCreateRequestModel requestModel)
        {
            var response = new CommonResponseModel();

            try
            {
                var fkValidation = await ValidateForeignKeysAsync(requestModel.JobId, requestModel.DeptId);
                if (fkValidation != null)
                    return fkValidation;

                if (!string.IsNullOrWhiteSpace(requestModel.CandidateStatus) && !IsValidCandidateStatus(requestModel.CandidateStatus))
                {
                    response.responseCode = StatusCodes.Status400BadRequest;
                    response.message = "Invalid CandidateStatus.";
                    return response;
                }

                var entity = mapper.Map<CandidateEntity>(requestModel);
                entity.CreatedOn = DateTime.Now;
                entity.Status = string.IsNullOrWhiteSpace(requestModel.Status) ? "Active" : requestModel.Status;
                entity.CandidateStatus = string.IsNullOrWhiteSpace(requestModel.CandidateStatus) ? "Applied" : requestModel.CandidateStatus;

                if (requestModel.Resume != null)
                {
                    var resumePath = await SaveResumeAsync(requestModel.Resume);
                    entity.CVPath = resumePath;
                }

                var result = await candidateRepository.AddAsync(entity);
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

        public async Task<CandidateSearchResponseModel?> SearchCandidateAsync(CandidateSearchRequestModel requestModel, string? offset, string count)
        {
            CandidateSearchResponseEntity entityResponse = await candidateRepository.SearchCandidateAsync(requestModel, offset, count);
            CandidateSearchResponseModel response = mapper.Map<CandidateSearchResponseModel>(entityResponse);
            return response;
        }

        public async Task<CommonResponseModel> UpdateCandidateAsync(Guid id, CandidateUpdateRequestModel requestModel)
        {
            CommonResponseModel responseModel = new CommonResponseModel();

            try
            {
                CandidateEntity? entity = await candidateRepository.FindAsync(id);

                if (entity == null)
                {
                    responseModel.responseCode = StatusCodes.Status400BadRequest;
                    responseModel.message = "Data Not Found!";
                    return responseModel;
                }

                var nextJobId = requestModel.JobId ?? entity.JobId;
                var nextDeptId = requestModel.DeptId ?? entity.DeptId;
                var fkValidation = await ValidateForeignKeysAsync(nextJobId, nextDeptId);
                if (fkValidation != null)
                    return fkValidation;

                if (!string.IsNullOrWhiteSpace(requestModel.CandidateStatus) && !IsValidCandidateStatus(requestModel.CandidateStatus))
                {
                    responseModel.responseCode = StatusCodes.Status400BadRequest;
                    responseModel.message = "Invalid CandidateStatus.";
                    return responseModel;
                }

                if (requestModel.JobId.HasValue)
                    entity.JobId = requestModel.JobId.Value;

                if (requestModel.DeptId.HasValue)
                    entity.DeptId = requestModel.DeptId.Value;

                if (!string.IsNullOrWhiteSpace(requestModel.FullName))
                    entity.FullName = requestModel.FullName;

                if (!string.IsNullOrWhiteSpace(requestModel.Email))
                    entity.Email = requestModel.Email;

                if (!string.IsNullOrWhiteSpace(requestModel.Phone))
                    entity.Phone = requestModel.Phone;

                if (!string.IsNullOrWhiteSpace(requestModel.Gender))
                    entity.Gender = requestModel.Gender;

                if (!string.IsNullOrWhiteSpace(requestModel.Education))
                    entity.Education = requestModel.Education;

                if (!string.IsNullOrWhiteSpace(requestModel.University))
                    entity.University = requestModel.University;

                if (!string.IsNullOrWhiteSpace(requestModel.CurrentTitle))
                    entity.CurrentTitle = requestModel.CurrentTitle;

                if (!string.IsNullOrWhiteSpace(requestModel.CurrentCompany))
                    entity.CurrentCompany = requestModel.CurrentCompany;

                if (!string.IsNullOrWhiteSpace(requestModel.Summary))
                    entity.Summary = requestModel.Summary;

                if (requestModel.ExperienceYears.HasValue)
                    entity.ExperienceYears = requestModel.ExperienceYears.Value;

                if (!string.IsNullOrWhiteSpace(requestModel.Location))
                    entity.Location = requestModel.Location;

                if (!string.IsNullOrWhiteSpace(requestModel.Skills))
                    entity.Skills = requestModel.Skills;

                if (requestModel.CurrentSalary.HasValue)
                    entity.CurrentSalary = requestModel.CurrentSalary;

                if (requestModel.ExpectedSalary.HasValue)
                    entity.ExpectedSalary = requestModel.ExpectedSalary;

                if (!string.IsNullOrWhiteSpace(requestModel.LinkedInProfile))
                    entity.LinkedInProfile = requestModel.LinkedInProfile;

                if (requestModel.Resume != null)
                {
                    var resumePath = await SaveResumeAsync(requestModel.Resume);
                    entity.CVPath = resumePath;
                }

                if (requestModel.AIScore.HasValue)
                    entity.AIScore = requestModel.AIScore;

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                    entity.Status = requestModel.Status;

                if (!string.IsNullOrWhiteSpace(requestModel.CandidateStatus))
                    entity.CandidateStatus = requestModel.CandidateStatus;

                entity.ModifiedOn = DateTime.Now;
                entity.ModifiedBy = requestModel.ActionBy;

                await candidateRepository.UpdateAsync(entity);

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

        private async Task<CommonResponseModel?> ValidateForeignKeysAsync(Guid jobId, Guid deptId)
        {
            bool isJobValid = await referenceValidationRepository.IsReferenceValidAsync(Models.Enums.ReferenceType.Job, jobId);
            if (!isJobValid)
            {
                return new CommonResponseModel
                {
                    responseCode = StatusCodes.Status400BadRequest,
                    message = "Invalid JobId."
                };
            }

            bool isDeptValid = await referenceValidationRepository.IsReferenceValidAsync(Models.Enums.ReferenceType.Department, deptId);
            if (!isDeptValid)
            {
                return new CommonResponseModel
                {
                    responseCode = StatusCodes.Status400BadRequest,
                    message = "Invalid DeptId."
                };
            }

            return null;
        }

        private static bool IsValidCandidateStatus(string status)
            => AllowedCandidateStatuses.Contains(status.Trim());

        private static async Task<string> SaveResumeAsync(IFormFile resume)
        {
            var extension = Path.GetExtension(resume.FileName).ToLower();
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid resume type. Only pdf/doc/docx are allowed.");

            if (resume.Length > 5 * 1024 * 1024)
                throw new Exception("Resume size should not exceed 5MB.");

            var folderPath = Path.Combine("wwwroot", "uploads", "candidates");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var fullPath = Path.Combine(folderPath, uniqueFileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await resume.CopyToAsync(stream);
            }

            return $"/uploads/candidates/{uniqueFileName}";
        }
    }
}
