using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourseProject.Attributes;

namespace CourseProject.Models.DataModels;

[TableName("Delivery")]
public class Delivery : Entity
{
    public Delivery()
    {
        DeliveryType = new DeliveryType();
        DeliveryProvider = new DeliveryProvider();
    }

    [Required][DisplayName("Address")] public string Address { get; set; }

    public DateTime Date { get; set; }

    [ForeignKey("provider_id")] public DeliveryProvider DeliveryProvider { get; set; }

    [ForeignKey("type_id")] public DeliveryType DeliveryType { get; set; }

    [ForeignKey("order_id")] public Order Order { get; set; }
}