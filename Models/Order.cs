using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    public class Order
    {
        [Display(Name = "订单编号")]
        public int Id { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        [Display(Name = "订单状态")]
        public OrderStatus Status { get; set; }
        [JsonIgnore]
        public string StatusCn
        {
            get
            {
                return EnumUtil.GetDescription(Status);
            }
        }
        public int ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public UserAddress ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public double TotalPrice { get { return OrderItems.Sum(i => i.Product.Price * i.Count); } }
    }

    public enum OrderStatus
    {
        [EnumDescription("已创建")]
        Created = 0,
        [EnumDescription("已支付")]
        Payed = 1,
        [EnumDescription("已发货")]
        Deliver = 2,
        [EnumDescription("已完成")]
        Completed = 3
    }
}
