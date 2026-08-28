using AutoMapper;
using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baslets;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Servicies
{
    internal class BasketService : IBasketService


    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
           _mapper = mapper;
        }

        public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, TimeSpan? TLV = null, CancellationToken ct = default)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
          var result= await _basketRepository.CreateOrUpdateBasketAsync(customerBasket,TLV, ct);


            return result == null ? Result<BasketDto>.Fail(Error.Failure("BasketCreate.Failure", "Can't create Or Update Basket")) :
                Result<BasketDto>.Ok(basket);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
          var result= await _basketRepository.RemoveItemFromBasketAsync(basketId, ct);
            return result? Result<bool>.Ok(true):Result<bool>.Fail(Error.Failure("BasketDelete.Failure","Can;t Delete Baket"));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string basketId, CancellationToken ct = default)
        {

           var basket=await _basketRepository.GetCustomerBasketAsync(basketId, ct);
            return basket == null ? Result<BasketDto>.Fail(Error.NotFound()) : _mapper.Map<BasketDto>(basket);
        }
    }
}
