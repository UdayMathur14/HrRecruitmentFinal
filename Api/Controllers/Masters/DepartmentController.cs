using BusinessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModels.Masters.Department;

namespace Api.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(IDepartmentService departmentService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await departmentService.GetByIdAsync(id);

            if (data == null)
                return BadRequest(new { code = 400, message = "Data Not Found!" });

            return Ok(data);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] DepartmentCreateRequestModel Deptmodel)
        {
            var data = await departmentService.CreateFullDepartmentAsync(Deptmodel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }

        [HttpPost("search")]
        public async Task<ActionResult> Search(DepartmentSearchRequestModel requestModel, [FromQuery] string? offset = null, [FromQuery] string? count = null)
        {
            var result = await departmentService.SearchDeptAsync(requestModel, offset, count!);
            if (result?.responseCode == 400)
                return BadRequest(result);
            return Ok(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("getAllDeptWithUsers")]
        public async Task<IActionResult> GetAll()
        {
            var data = await departmentService.GetAllAsync();
            return Ok(data);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DepartmentUpdateRequestModel requestModel)
        {
            var data = await departmentService.UpdateDepartmentAsync(id, requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }

    }

}
