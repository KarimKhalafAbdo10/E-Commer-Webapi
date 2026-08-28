using E_Commerce.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class OrderSpecification :BaseSpecification<Order ,Guid>
    {
        public OrderSpecification(string email ):base(x=>x.BuyerEmail==email)
        {
            AddInclude(x=>x.DeliveryMethod);
            AddInclude(x=>x.Items);
            AddOrederByDescending(x => x.OrderDate);
        }
        public OrderSpecification(Guid id,string email ):base(x=>x.BuyerEmail==email&& x.Id==id)
        {
            AddInclude(x=>x.DeliveryMethod);
            AddInclude(x=>x.Items);
           
        }

    }
}
