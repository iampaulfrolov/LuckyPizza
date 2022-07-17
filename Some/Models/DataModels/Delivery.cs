using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CourseProject.Attributes;

namespace CourseProject.Models.DataModels
{
    [TableName("Delivery")]
    public class Delivery : Entity
    {
        [DisplayName("Адреса доставки")]
        public string Address { get; set; }

        public DateTime Date { get; set; }

        public int Parcel_number { get; set; }

        [ForeignKey("provider_id")] public DeliveryProvider DeliveryProvider { get; set; }

        [ForeignKey("type_id")] public DeliveryType DeliveryType { get; set; }

        [ForeignKey("order_id")] public Order Order { get; set; }

        public Delivery()
        {
            DeliveryType = new DeliveryType();
            DeliveryProvider = new DeliveryProvider();
        }
    }
}