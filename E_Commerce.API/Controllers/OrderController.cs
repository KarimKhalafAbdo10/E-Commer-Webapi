using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    
    public class OrderController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region Create Order
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto order , CancellationToken ct)
        {
            return ToActionResult(await  _orderService.CreateOrder(order, GetEmailFromToken(), ct));
        }
        #endregion
        
        

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders(CancellationToken ct)
        => ToActionResult(await _orderService.GetAllOrdersForUser(GetEmailFromToken(),ct));
        
        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>>GetOrderById(Guid id, CancellationToken ct)
        =>  ToActionResult( await _orderService.GetOrderByIdAndEmailForUser(id, GetEmailFromToken(), ct));
        

        [AllowAnonymous]
        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>>GetDeliveryMethod(CancellationToken ct)
        => ToActionResult(await _orderService.GetDeliveryMethodsAsync(ct));
        
    }
};
