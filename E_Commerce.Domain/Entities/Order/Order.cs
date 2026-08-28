using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Order
{
    public class Order:BaseEntity<Guid>
    {
        public Order(string buyerEmail, ICollection<OrderItem> items, OrderAddress shippingAddress, decimal subTotal, DeliveryMethod deliveryMethod, string paymentIntentId )
        {
            BuyerEmail = buyerEmail;
            Items = items;
            ShippingAddress = shippingAddress;
            SubTotal = subTotal;
            DeliveryMethod = deliveryMethod;
            PaymentIntentId = paymentIntentId;
        }

        private Order()
        {
            
        }
        public string? PaymentIntentId { get; set; } = default!;
        public string BuyerEmail { get; set; } = default!;
        public ICollection<OrderItem> Items { get; set; } = [];
        public OrderAddress ShippingAddress { get; set; } = default!;
        public decimal SubTotal { get;private set; }
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.UtcNow;
        public OrederStatus Status { get; set; } = OrederStatus.Pending;

        public decimal GetTotal()=> SubTotal+(DeliveryMethod?.Cost??0m);


    }
}
