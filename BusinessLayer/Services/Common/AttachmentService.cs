using AutoMapper;
using BusinessLayer.Interfaces.Common;
using DataAccessLayer.Domain.Common.Attachments;
using DataAccessLayer.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Models;
using Models.Enums;
using Models.RequestModels.Common.Attachments;
using Models.ResponseModels.Common.Attachments;

namespace BusinessLayer.Services.Common
{
    public class AttachmentService(IAttachmentRepository attachmentRepository, IReferenceValidationRepository referenceValidationRepository, IMapper mapper) : IAttachmentService
    {
        private static bool IsValidReferenceType(ReferenceType referenceType)
            => Enum.IsDefined(typeof(ReferenceType), referenceType);

        public async Task<AttachmentReadResponseModel?> GetByIdAsync(Guid id)
        {
            AttachmentEntity? entity = await attachmentRepository.FindAsync(id);

            if (entity == null)
                return null;

            if (!IsValidReferenceType((ReferenceType)entity.ReferenceType))
                return null;

            var referenceType = (ReferenceType)entity.ReferenceType;
            bool isValidReference = await referenceValidationRepository.IsReferenceValidAsync(referenceType, entity.ReferenceId);
            if (!isValidReference)
                return null;

            AttachmentReadResponseModel response = mapper.Map<AttachmentReadResponseModel>(entity);
            return response;
        }

        public async Task<CommonResponseModel> CreateAttachmentAsync(AttachmentCreateRequestModel requestModel)
        {
            var response = new CommonResponseModel();

            try
            {
                // ✅ 1. Validate ReferenceType
                if (!Enum.IsDefined(typeof(ReferenceType), requestModel.ReferenceType))
                {
                    response.responseCode = StatusCodes.Status400BadRequest;
                    response.message = "Invalid ReferenceType.";
                    return response;
                }

                // ✅ 2. Validate ReferenceId (Dept / Job exist karta hai ya nahi)
                bool isValidReference = await referenceValidationRepository
                    .IsReferenceValidAsync(requestModel.ReferenceType, requestModel.ReferenceId);

                if (!isValidReference)
                {
                    response.responseCode = StatusCodes.Status400BadRequest;
                    response.message = $"ReferenceId not found for {requestModel.ReferenceType}.";
                    return response;
                }

                // ✅ 3. Validate File
                if (requestModel.File == null || requestModel.File.Length == 0)
                {
                    response.responseCode = StatusCodes.Status400BadRequest;
                    response.message = "File is required.";
                    return response;
                }

                // ✅ 4. File Extension Check
                var extension = Path.GetExtension(requestModel.File.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx" };

                if (!allowedExtensions.Contains(extension))
                {
                    response.responseCode = StatusCodes.Status400BadRequest;
                    response.message = "Invalid file type.";
                    return response;
                }

                // ✅ 5. File Size Check (5MB)
                if (requestModel.File.Length > 5 * 1024 * 1024)
                {
                    response.responseCode = StatusCodes.Status400BadRequest;
                    response.message = "File size should not exceed 5MB.";
                    return response;
                }

                // ✅ 6. Create Folder if not exists
                var folderPath = Path.Combine("wwwroot", "uploads");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // ✅ 7. Generate Unique File Name
                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                var fullPath = Path.Combine(folderPath, uniqueFileName);

                // ✅ 8. Save File
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await requestModel.File.CopyToAsync(stream);
                }

                // ✅ 9. Prepare Entity
                var entity = new AttachmentEntity
                {
                    Id = Guid.NewGuid(),
                    ReferenceType = (int)requestModel.ReferenceType,
                    ReferenceId = requestModel.ReferenceId,
                    FilePath = "/uploads/" + uniqueFileName,
                    FileName = requestModel.File.FileName,
                    CreatedBy = requestModel.CreatedBy,
                    CreatedOn = DateTime.Now,
                    Status = "Active"
                };

                // ✅ 10. Save to DB
                var result = await attachmentRepository.AddAsync(entity);

                // ✅ 11. Success Response
                response.responseCode = StatusCodes.Status200OK;
                response.message = "File uploaded successfully.";
                response.Id = result;
            }
            catch (Exception ex)
            {
                response.responseCode = StatusCodes.Status500InternalServerError;
                response.message = "Something went wrong: " + ex.Message;
            }

            return response;
        }

        public async Task<AttachmentSearchResponseModel?> SearchAttachmentAsync(AttachmentSearchRequestModel requestModel, string? offset, string count)
        {
            if (!requestModel.ReferenceType.HasValue && requestModel.ReferenceId.HasValue)
            {
                return new AttachmentSearchResponseModel
                {
                    responseCode = StatusCodes.Status400BadRequest,
                    message = "ReferenceType is required when ReferenceId is provided."
                };
            }

            if (requestModel.ReferenceType.HasValue && !IsValidReferenceType(requestModel.ReferenceType.Value))
            {
                    return new AttachmentSearchResponseModel
                    {
                        responseCode = StatusCodes.Status400BadRequest,
                        message = "Invalid ReferenceType. Allowed values: Department, Job, or Candidate."
                    };
                }

            if (requestModel.ReferenceType.HasValue && requestModel.ReferenceId.HasValue)
            {
                var referenceType = requestModel.ReferenceType.Value;
                bool isValidReference = await referenceValidationRepository.IsReferenceValidAsync(referenceType, requestModel.ReferenceId.Value);
                if (!isValidReference)
                {
                    return new AttachmentSearchResponseModel
                    {
                        responseCode = StatusCodes.Status400BadRequest,
                        message = $"ReferenceId not found for {referenceType}."
                    };
                }
            }

            AttachmentSearchResponseEntity entityResponse = await attachmentRepository.SearchAttachmentAsync(requestModel, offset, count);
            AttachmentSearchResponseModel response = mapper.Map<AttachmentSearchResponseModel>(entityResponse);

            return response;
        }

        public async Task<CommonResponseModel> UpdateAttachmentAsync(Guid id, AttachmentUpdateRequestModel requestModel)
        {
            CommonResponseModel responseModel = new CommonResponseModel();

            try
            {
                AttachmentEntity? entity = await attachmentRepository.FindAsync(id);

                if (entity == null)
                {
                    responseModel.responseCode = StatusCodes.Status400BadRequest;
                    responseModel.message = "Data Not Found!";
                    return responseModel;
                }

                if (!string.IsNullOrWhiteSpace(requestModel.FilePath))
                    entity.FilePath = requestModel.FilePath;

                if (!string.IsNullOrWhiteSpace(requestModel.FileName))
                    entity.FileName = requestModel.FileName;

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                    entity.Status = requestModel.Status;

                entity.ModifiedOn = DateTime.Now;
                entity.ModifiedBy = requestModel.ActionBy;

                await attachmentRepository.UpdateAsync(entity);

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
