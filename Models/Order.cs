using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Remark { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        public OrderStatus Status { get; set; }
        public int ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public UserAddress ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public double TotalPrice { get { return OrderItems.Sum(i => i.Product.Price * i.Count); } }
    }

    public enum OrderStatus
    {
        Created = 0,
        Payed = 1,
        Deliver = 2,
        Completed = 3
    }
}
