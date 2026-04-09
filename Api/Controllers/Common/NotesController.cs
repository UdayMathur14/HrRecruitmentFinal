using BusinessLayer.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using Models.RequestModels.Common.Notes;

namespace Api.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController(INoteService noteService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await noteService.GetByIdAsync(id);

            if (data == null)
                return BadRequest(new { code = 400, message = "Data Not Found!" });

            return Ok(data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NoteCreateRequestModel requestModel)
        {
            var data = await noteService.CreateNoteAsync(requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }

        [HttpPost("search")]
        public async Task<ActionResult> Search(NoteSearchRequestModel requestModel, [FromQuery] string? offset = null, [FromQuery] string? count = null)
        {
            var result = await noteService.SearchNoteAsync(requestModel, offset, count!);
            if (result?.responseCode == 400)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] NoteUpdateRequestModel requestModel)
        {
            var data = await noteService.UpdateNoteAsync(id, requestModel);
            if (data.responseCode == 400)
                return BadRequest(data);
            return Ok(data);
        }
    }
}
