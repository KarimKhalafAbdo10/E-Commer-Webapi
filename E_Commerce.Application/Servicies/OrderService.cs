using AutoMapper;
using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Order;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Servicies
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketService basketService,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<OrderToReturnDto>> CreateOrder(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            var basket = await _basketService.GetBasketAsync(orderDto.BasketId, ct);
            if (!basket.IsSuccess)
                return Result<OrderToReturnDto>.Fail(basket.Errors);

            if (basket.data.Items.Count <= 0)
                return Result<OrderToReturnDto>.Fail(Error.Failure("Basket Is Empty", $"Can't create Order With BasketId {basket.data.Id}"));

            var exOrder =await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new PaymentIntentIdSpeci(basket.data.PaymentIntentId));
            if (exOrder != null)
                _unitOfWork.GetRepository<Order, Guid>().Delete(exOrder);

            var orderItems = new List<OrderItem>(basket.data.Items.Count);
            var productIds = basket.data.Items.Select(p => p.Id).ToHashSet();
            var products = (await _unitOfWork.GetRepository<Product, int>().GetAllAsync(new ProductWithIdSpecification(productIds), ct)).ToDictionary(x => x.Id);

            foreach (var item in basket.data.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<OrderToReturnDto>.Fail(Error.NotFound("Product Not Found", $"Product With ID {item.Id} Not found"));

                orderItems.Add(new OrderItem()
                {
                    Price = product.Price,
                    Quantity = item.Quantity,
                    Product = new ProductItemOrdred()
                    {
                        PictureUrl = product.PictureUrl,
                        ProductId = product.Id,
                        ProductName = product.Name,
                    }
                });
            }

            var orderAddress = _mapper.Map<OrderAddress>(orderDto.ShippingAddress);
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);

            if (deliveryMethod is null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("DeliveryMethod Is Not Found", $"DeliveryMethod With Id {orderDto.DeliveryMethodId} Is Not Found"));

            var subTotal = orderItems.Sum(O => O.Quantity * O.Price);

            var order = new Order(email, orderItems, orderAddress, subTotal, deliveryMethod,basket.data.PaymentIntentId);
            _unitOfWork.GetRepository<Order, Guid>().Add(order);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result == 0)
            {
                return Result<OrderToReturnDto>.Fail(Error.Failure("Order Fail", "Can't create Order"));
            }
            else
            {
                await _basketService.DeleteBasketAsync(orderDto.BasketId);
                return Result<OrderToReturnDto>.Ok(_mapper.Map<OrderToReturnDto>(order));
            }
        }

        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUser(string email, CancellationToken ct = default)
        {
            var orders =await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecification(email),ct);

            if (orders.Any())
            {
                return Result<IReadOnlyList<OrderToReturnDto>>.Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
            }
            else
            {
                return Error.NotFound("Order Not Found",$"No Order Was Found For User With Email {email}");
            }
                
                
                
                }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync(CancellationToken ct = default)
        {
            var deliveryMethod =await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(ct);

            if (deliveryMethod.Any())
       
                return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(_mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethod));

            return Error.NotFound("DeliveryMethod Not Found");
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailForUser(Guid id, string email, CancellationToken ct = default)
        {
            var order =await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecification(id,email));
            if(order is null)
            {
               return Error.NotFound("Order not Found ",$"Order Wth Id {id} Not Found");
            }
            else
            {
                return Result<OrderToReturnDto>.Ok(_mapper.Map<OrderToReturnDto>(order));
            }
                
                }
    }
}
