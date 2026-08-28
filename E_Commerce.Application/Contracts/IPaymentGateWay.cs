using E_Commerce.Application.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentGateWay

    {


        Task<Result<PaymentIntentResult>> CreatePaymentIntentAsync(decimal amount,string currency,CancellationToken ct=default);
        Task<Result<PaymentIntentResult>> UpdatePaymentIntentAsync(string paymentIntentId,decimal amount,CancellationToken ct=default);
    }
}
