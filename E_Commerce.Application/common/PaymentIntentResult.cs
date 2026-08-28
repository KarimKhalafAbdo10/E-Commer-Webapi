using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.common
{
    public sealed class PaymentIntentResult
    {
        public PaymentIntentResult(string clientSecret, string paymentIntentId)
        {
            ClientSecret = clientSecret;
            PaymentIntentId = paymentIntentId;
        }

        public string ClientSecret { get;  } = default!;
        public string PaymentIntentId { get;  } = default!;
    }
}
