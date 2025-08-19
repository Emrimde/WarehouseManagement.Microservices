using Microsoft.AspNetCore.Mvc;

namespace PickingMicroservice.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PickingController : ControllerBase
{
    [HttpGet("tasks")] 
    public async Task<IActionResult> GetAllTasks()
    {
        throw new NotImplementedException();
    }
    [HttpGet("tasks/{orderId}")]
    public async Task<IActionResult> GetTaskById(Guid orderId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("tasks/{orderId}/complete")]
    public async Task<IActionResult> PostTaskCompleted(Guid orderId)
    {
        throw new NotImplementedException();
    }
}
