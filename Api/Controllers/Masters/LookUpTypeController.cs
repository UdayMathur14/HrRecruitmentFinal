using BusinessLogic.Interfaces.Masters;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModels.Masters.LookUpType;

namespace Api.Controllers.V1.Masters
{
    [Route("api/[controller]")]
    [ApiController]

    public class LookUpTypeController(ILookUpTypeService lookUpTypeService) : ControllerBase
    {
    
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var result = await lookUpTypeService.GetLookUpTypeAsync(id);
            if (result == null)
                return BadRequest(new { code = 400, message = "Data Not Found!" });
            return Ok(result);
        }

    
        [HttpPost("search")]
        public async Task<ActionResult> Search(LookUpTypeSearchRequestModel requestModel, [FromQuery] string? offset = null, [FromQuery] string? count = null)
        {
            var result = await lookUpTypeService.SearchLookUpAsync(requestModel, offset, count!);
            if (result?.responseCode == 400)
                return BadRequest(result);
            return Ok(result);
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, LookUpTypeRequestModel lookUp)
        {
            var result = await lookUpTypeService.UpdateLookUpTypeAsync(lookUp, id);
            if (result?.responseCode == 400)
                return BadRequest(result);
            return Ok(result);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create(LookUpTypeRequestModel lookUp)
        {
            var result = await lookUpTypeService.CreateLookUpAsync(lookUp);
            if (result?.responseCode == 400)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
