using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Payments
{
    public class StripePaymentGateway : IPaymentGateWay
    {

        private readonly PaymentIntentService _paymentIntentService = new();
        public StripePaymentGateway(IOptions<PaymentGatewaySettings>options)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey;       
                
                }
        public async Task<Result<PaymentIntentResult>> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var options = new PaymentIntentCreateOptions()
            {
                Currency = currency.ToLower(),
                Amount = (long)amount,
                PaymentMethodTypes = ["card"],

            };



         var intent=  await _paymentIntentService.CreateAsync(options,cancellationToken:ct);

            return new PaymentIntentResult(intent.Id,intent.ClientSecret);
                }

        public async Task<Result<PaymentIntentResult>> UpdatePaymentIntentAsync(string paymentIntentId, decimal amount, CancellationToken ct = default)
        {
            var options = new PaymentIntentUpdateOptions()
            {
                 Amount = (long)amount,

            };
          var intent= await _paymentIntentService.UpdateAsync(paymentIntentId,options,cancellationToken:ct);
            return new PaymentIntentResult(intent.Id,intent.ClientSecret);
                
                
                }
    }
}
