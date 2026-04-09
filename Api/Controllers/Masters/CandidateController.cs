using BusinessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModels.Masters.Candidate;

namespace Api.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController(ICandidateService candidateService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await candidateService.GetByIdAsync(id);

            if (data == null)
                return BadRequest(new { code = 400, message = "Data Not Found!" });

            data.CVPath = ToAbsoluteCvUrl(data.CVPath);
            return Ok(data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CandidateCreateRequestModel requestModel)
        {
            var data = await candidateService.CreateCandidateAsync(requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }

        [HttpPost("search")]
        public async Task<ActionResult> Search(CandidateSearchRequestModel requestModel, [FromQuery] string? offset = null, [FromQuery] string? count = null)
        {
            var result = await candidateService.SearchCandidateAsync(requestModel, offset, count ?? "10");
            if (result?.responseCode == 400)
                return BadRequest(result);

            if (result?.Candidates != null)
            {
                foreach (var candidate in result.Candidates)
                {
                    candidate.CVPath = ToAbsoluteCvUrl(candidate.CVPath);
                }
            }

            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CandidateUpdateRequestModel requestModel)
        {
            var data = await candidateService.UpdateCandidateAsync(id, requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }

        private string? ToAbsoluteCvUrl(string? cvPath)
        {
            if (string.IsNullOrWhiteSpace(cvPath))
                return cvPath;

            if (Uri.TryCreate(cvPath, UriKind.Absolute, out _))
                return cvPath;

            var normalizedPath = cvPath.Replace("\\", "/");
            if (!normalizedPath.StartsWith("/"))
                normalizedPath = "/" + normalizedPath;

            return $"{Request.Scheme}://{Request.Host}{normalizedPath}";
        }
    }
}
