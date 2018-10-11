using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class Order
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        Created = 0,
        Payed = 1,
        Deliver = 2,
        Completed = 3
    }
}
