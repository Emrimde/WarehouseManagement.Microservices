using Microsoft.AspNetCore.Mvc;
using OrderMicroservice.Core.Domain.Entities;
using OrderMicroservice.Core.DTO.OrderDTO;
using OrderMicroservice.Core.Result;
using OrderMicroservice.Core.ServiceContracts;

namespace OrderMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            IEnumerable<OrderResponse> response = await _orderService.GetAllOrders();
            return Ok(response);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            Result<OrderResponse> response = await _orderService.GetOrderById(id);
            if (response.IsSuccess)
            {
                return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
            }
            return Ok(response.Value);
        }

        [HttpGet("order-status/{id}")]
        public async Task<ActionResult> GetOrderStatus(Guid id)
        {
            Result<string> response = await _orderService.GetOrderStatusById(id);
            if (response.IsSuccess)
            {
                return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
            }
            return Ok(response.Value);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> PostOrder([FromBody] OrderAddRequest order)
        {
            Result<OrderResponse> response = await _orderService.AddOrder(order);
            if (!response.IsSuccess)
            {
                return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
            }
            return Ok(response.Value);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            Result<bool> response = await _orderService.DeleteOrder(id);
            if (response.IsSuccess)
            {
                return Problem(detail: response.Message, statusCode: (int)response.StatusCode);
            }
            return NoContent();
        }
    }
}
