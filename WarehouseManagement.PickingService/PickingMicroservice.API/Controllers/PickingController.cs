using Microsoft.AspNetCore.Mvc;
using PickingMicroservice.Core.DTO;
using PickingMicroservice.Core.Result;
using PickingMicroservice.Core.ServiceContracts;

namespace PickingMicroservice.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PickingController : ControllerBase
{
    private readonly IPickingService _pickingService;
    public PickingController(IPickingService pickingService)
    {
        _pickingService = pickingService;
    }
    [HttpGet("tasks")] 
    public async Task<ActionResult> GetAllTasks()
    {
        IEnumerable<PickingResponse> response = await _pickingService.GetAllTasks();
        return Ok(response);
    }
    [HttpGet("tasks/{orderId}")]
    public async Task<ActionResult<PickingResponse>> GetTaskById(string orderId)
    {
        Result<PickingResponse> response = await _pickingService.GetTaskById(orderId);
        if (!response.IsSuccess)
        {
            return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
        }
        return Ok(response.Value);
    }

    [HttpPost("tasks/{orderId}/complete")]
    public async Task<ActionResult<bool>> PostTaskCompleted(string orderId)
    {
        Result<bool> response = await _pickingService.MakeTaskCompleted(orderId);
        if (!response.IsSuccess)
        {
            return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
        }
        return Ok(response.Value);
    }
}
