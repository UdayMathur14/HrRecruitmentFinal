using BusinessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModels.Masters.User;

namespace Api.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await userService.GetByIdAsync(id);

            if (data == null)
                return BadRequest(new { code = 400, message = "Data Not Found!" });

            return Ok(data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequestModel requestModel)
        {
            var data = await userService.CreateUserAsync(requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }

        [HttpPost("search")]
        public async Task<ActionResult> Search(UserSearchRequestModel requestModel, [FromQuery] string? offset = null, [FromQuery] string? count = null)
        {
            var result = await userService.SearchUserAsync(requestModel, offset, count!);
            if (result?.responseCode == 400)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequestModel requestModel)
        {
            var data = await userService.UpdateUserAsync(id, requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }
    }
}
