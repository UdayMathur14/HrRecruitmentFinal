using BusinessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModels.Masters.DepartmentSummary;

namespace Api.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentSummaryController(IDepartmentSummaryService departmentSummaryService) : ControllerBase
    {
        [HttpPost("summary")]
        public async Task<IActionResult> GetSummary([FromBody] DepartmentSummaryRequestModel requestModel)
        {
            var data = await departmentSummaryService.GetSummaryAsync(requestModel);

            if (data.responseCode == 400)
                return BadRequest(data);

            return Ok(data);
        }
    }
}
