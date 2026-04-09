using BusinessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModels.Masters.Job;

namespace Api.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController(IJobService jobService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await jobService.GetByIdAsync(id);

            if (data == null)
                return BadRequest(new { code = 400, message = "Data Not Found!" });

            return Ok(data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] JobCreateRequestModel requestModel)
        {
            var data = await jobService.CreateJobAsync(requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }

        [HttpPost("search")]
        public async Task<ActionResult> Search(JobSearchRequestModel requestModel, [FromQuery] string? offset = null, [FromQuery] string? count = null)
        {
            var result = await jobService.SearchJobAsync(requestModel, offset, count!);
            if (result?.responseCode == 400)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] JobUpdateRequestModel requestModel)
        {
            var data = await jobService.UpdateJobAsync(id, requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }
    }
}
