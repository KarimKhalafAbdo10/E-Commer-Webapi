using E_Commerce.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class PaymentIntentIdSpeci:BaseSpecification<Order,Guid>
    {
        public PaymentIntentIdSpeci(string paymentIntentId):base(x=>x.PaymentIntentId==paymentIntentId)
        {
            
        }

    }
}
