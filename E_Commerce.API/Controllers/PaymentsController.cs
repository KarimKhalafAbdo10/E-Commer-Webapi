using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baslets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.API.Controllers
{

    public class PaymentsController: ApiBaseController
    {
        private readonly IPaymentService  _paymentService;
        private readonly PaymentGatewaySettings _paymentGatewaySettings;
        public PaymentsController(IOptions<PaymentGatewaySettings>options,IPaymentService paymentService)
        {
            _paymentGatewaySettings = options.Value;
            _paymentService = paymentService;
        }


        [Authorize]
        [HttpPost("{bsketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string bsketId,CancellationToken ct)
        {
            return ToActionResult(await _paymentService.CreateOrUpdatePaymentIntentAsync(bsketId,ct));
        }



        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var requestJson =await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(requestJson, Request.Headers["Stripe-Signature"],_paymentGatewaySettings.WebhookSecret);

                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        if (paymentIntent is not null)
                           await _paymentService.PaymentSucceeded(paymentIntent.Id);

                        break;

                    case EventTypes.PaymentIntentPaymentFailed:
                        var paymentIntentFailed = stripeEvent.Data.Object as PaymentIntent;
                        if (paymentIntentFailed is not null)
                            await _paymentService.PaymentFailed(paymentIntentFailed.Id);

                        break;

                    default:
                        break;

                }

                return Ok();
                    
            }
            catch(StripeException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }

        }

    }
}
