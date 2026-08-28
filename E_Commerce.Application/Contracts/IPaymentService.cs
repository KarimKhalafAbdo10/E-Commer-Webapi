using E_Commerce.Application.common;
using E_Commerce.Application.DTOs.Baslets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId,CancellationToken  ct=default);
        Task PaymentSucceeded(string paymentIntentId);
        Task PaymentFailed(string paymentIntentId);

    }
}
