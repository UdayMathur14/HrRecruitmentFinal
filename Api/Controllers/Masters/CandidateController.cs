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
    }
}
