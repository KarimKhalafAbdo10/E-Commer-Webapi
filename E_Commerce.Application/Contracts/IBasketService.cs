using E_Commerce.Application.common;
using E_Commerce.Application.DTOs.Baslets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IBasketService
    {
        Task<Result<BasketDto>> GetBasketAsync(string basketId, CancellationToken ct = default);
        Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket,TimeSpan? TLV =default! , CancellationToken ct = default);
        Task<Result<bool>> DeleteBasketAsync(string basketId,CancellationToken ct = default);
    }
}
