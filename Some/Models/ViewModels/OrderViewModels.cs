using System.Collections.Generic;
using CourseProject.Models.DataModels;

namespace CourseProject.Models.ViewModels;

public class OrderCreateViewModel
{
    public List<DeliveryProvider> DeliveryProviders { get; set; }
    public List<DeliveryType> DeliveryTypes { get; set; }
    public Delivery Delivery { get; set; }
}

public class OrderConfirmViewModel
{
    public List<Order> Orders { get; set; }
    public List<OrderStatus> StatusList { get; set; }
}