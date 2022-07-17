using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CourseProject.Attributes;
using System;

namespace CourseProject.Models.DataModels
{
    [TableName("Order")]
    public class Order : Entity
    {
        [ForeignKey("user_id")]
        public User User { get; set; }

        [ForeignKey("orderstatus_id")]
        public OrderStatus Status { get; set; }

        public DateTime Date  { get; set; }

        [ForeignKeyToMany("Product")] 
        public List<OrderProduct> Products { get; set; }


        public Order()
        {
            User = new User();
            Products = new List<OrderProduct>();
            Status = new OrderStatus();
        }
    }
}