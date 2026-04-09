using BusinessLayer.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModels.Common.Attachments;

namespace Api.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentsController(IAttachmentService attachmentService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await attachmentService.GetByIdAsync(id);

            if (data == null)
                return BadRequest(new { code = 400, message = "Data Not Found!" });

            data.FilePath = ToAbsoluteFileUrl(data.FilePath);
            return Ok(data);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Create([FromForm] AttachmentCreateRequestModel requestModel)
        {
            var data = await attachmentService.CreateAttachmentAsync(requestModel);

            if (data.responseCode == 400)
                return BadRequest(data);

            return Ok(data);
        }

        [HttpPost("search")]
        public async Task<ActionResult> Search(AttachmentSearchRequestModel requestModel, [FromQuery] string? offset = null, [FromQuery] string? count = null)
        {
            var result = await attachmentService.SearchAttachmentAsync(requestModel, offset, count ?? "10");
            if (result?.responseCode == 400)
                return BadRequest(result);

            if (result?.Attachments != null)
            {
                foreach (var attachment in result.Attachments)
                {
                    attachment.FilePath = ToAbsoluteFileUrl(attachment.FilePath);
                }
            }

            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AttachmentUpdateRequestModel requestModel)
        {
            var data = await attachmentService.UpdateAttachmentAsync(id, requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }

        private string? ToAbsoluteFileUrl(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return filePath;

            if (Uri.TryCreate(filePath, UriKind.Absolute, out _))
                return filePath;

            var normalizedPath = filePath.Replace("\\", "/");
            if (!normalizedPath.StartsWith("/"))
                normalizedPath = "/" + normalizedPath;

            return $"{Request.Scheme}://{Request.Host}{normalizedPath}";
        }
    }
}
