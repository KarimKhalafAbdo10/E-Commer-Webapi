using E_Commerce.Application.common;
using E_Commerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDto>> CreateOrder(OrderDto orderDto,string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUser(string email,CancellationToken ct =default);
        Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailForUser(Guid id , string email,CancellationToken ct =default);
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync( CancellationToken ct = default);

    }
}
