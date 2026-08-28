using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Order
{
    public enum OrederStatus
    {
        Pending=0,
        PaymentReceived=1,
        PaymentFailed=2


    }
}
