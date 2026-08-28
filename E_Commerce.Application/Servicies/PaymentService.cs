using AutoMapper;
using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baslets;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Order;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Servicies
{
    internal class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateWay _paymentGateWay;
        private readonly IMapper _mapper;
        private readonly PaymentGatewaySettings _paymentGatewaySettings;

        public PaymentService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentGateWay paymentGateWay,
            IOptions<PaymentGatewaySettings>options,
            IMapper mapper)
        {
           _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentGateWay = paymentGateWay;
            _mapper = mapper;
            _paymentGatewaySettings = options.Value;
        }
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {

            #region Get Basket And Validate

            var basket =await _basketRepository.GetCustomerBasketAsync(basketId,ct);

            if (basket is null)
                return Error.NotFound("Basket Not Found", $"Basket With Id {basketId} Is Not Found");
            if (basket.Items.Count== 0)
                return Error.Validation("Basket Is Empty");
            #endregion
            #region Get DeliveryMethod Cost
            if (!basket.DeliveryMethodId.HasValue)
                return Error.Validation("DeliveryMethod Id Is Required");
            var deliveryMethod =await _unitOfWork.GetRepository<DeliveryMethod,int>().GetByIdAsync(basket.DeliveryMethodId.Value);

            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod Is Not Found");

            basket.ShippingCost = deliveryMethod.Cost;
            #endregion

            #region ProductsPrice
            var productsIds = basket.Items.Select(p=>p.Id).ToHashSet();
            var products =(await _unitOfWork.GetRepository<Product, int>().GetAllAsync(new ProductWithIdSpecification(productsIds))).ToDictionary(p=>p.Id);

            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Error.NotFound("Product Not Found");

                item.Price= product.Price;
            }
            #endregion

            #region Total Price

            var subTotal = basket.Items.Sum(i=>i.Price * i.Quantity);
            var amount =(long)((subTotal + deliveryMethod.Cost) *100m);
            #endregion

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var result = await _paymentGateWay.CreatePaymentIntentAsync(amount, _paymentGatewaySettings.DefaultCurrency, ct);

                basket.PaymentIntentId = result.data.PaymentIntentId;
                basket.ClientSecret= result.data.ClientSecret;
            }
            else
            {
              var result= await _paymentGateWay.UpdatePaymentIntentAsync(basket.PaymentIntentId,amount,ct);
            }
          await  _basketRepository.CreateOrUpdateBasketAsync(basket,ct:ct);


            return Result<BasketDto>.Ok(_mapper.Map<BasketDto>(basket));
        }

        public async Task PaymentFailed(string paymentIntentId)
        {
            var order =await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new PaymentIntentIdSpeci(paymentIntentId));
            if (order == null)
                return;
            order.Status = OrederStatus.PaymentFailed;
          await  _unitOfWork.SaveChangesAsync();
                
        }

        public async Task PaymentSucceeded(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new PaymentIntentIdSpeci(paymentIntentId));
            if (order == null)
                return;
            order.Status = OrederStatus.PaymentReceived;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
