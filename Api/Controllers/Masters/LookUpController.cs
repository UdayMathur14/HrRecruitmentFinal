using BusinessLogic.Interfaces.Masters;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModels.Masters.LookUp;

namespace Api.Controllers.V1.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpController(ILookUpService lookUpService) : ControllerBase
    {
    
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var result = await lookUpService.GetLookUpAsync(id);
            if (result == null)
                return BadRequest(new { code = 400, message = "Data Not Found!" });
            return Ok(result);
        }

        [HttpPost("search/{loginUserId}")]
        public async Task<ActionResult> Search(LookUpSearchRequestModel requestModel, string loginUserId, [FromQuery] string? offset = null, [FromQuery] string? count = null)
        {
            var result = await lookUpService.SearchLookUpAsync(requestModel, loginUserId, offset, count!);
            if (result?.responseCode == 400)
                return BadRequest(result);
            return Ok(result);
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, LookUpRequestModel lookUp)
        {
            var result = await lookUpService.UpdateLookUpAsync(lookUp, id);
            if (result == null)
                return BadRequest(new { code = 400, message = "Data Not Found!" });
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(LookUpRequestModel lookUp)
        {
            var result = await lookUpService.CreateLookUpAsync(lookUp);
            if (result?.responseCode == 400)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
