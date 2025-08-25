using Microsoft.AspNetCore.Mvc;
using PickingMicroservice.Core.DTO;
using PickingMicroservice.Core.Result;
using PickingMicroservice.Core.Service;
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
        IEnumerable<PickTaskResponse> response = await _pickingService.GetAllTasks();
        return Ok(response);
    }

    [HttpGet("tasks/{pickTaskId}")]
    public async Task<ActionResult<PickTaskResponse>> GetTaskById(Guid pickTaskId)
    {
        Result<IEnumerable<PickItemResponse>> response = await _pickingService.GetTaskByOrderIdAsync(pickTaskId);
        if (!response.IsSuccess)
        {
            return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
        }
        return Ok(response.Value);
    }

    // will be implemented soon
    //[HttpPost("tasks/{orderId}/complete")]
    //public async Task<ActionResult<bool>> PostTaskCompleted(string orderId)
    //{
    //    Result<bool> response = await _pickingService.MakeTaskCompleted(orderId);
    //    if (!response.IsSuccess)
    //    {
    //        return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
    //    }
    //    return Ok(response.Value);
    //}
}
