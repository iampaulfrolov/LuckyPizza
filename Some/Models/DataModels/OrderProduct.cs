﻿using CourseProject.Attributes;

namespace CourseProject.Models.DataModels;

[RelatedTableName("Product")]
[RelatedEntityType("Product")]
[MasterEntityName("Order")]
[TransitionTableName("OrderProduct")]
public class OrderProduct : Product
{
    public OrderProduct(int quantity, decimal price)
    {
        _Quantity = quantity;
        _Price = price;
    }

    public OrderProduct()
    {
    }

    public int _Quantity { get; set; }
    public decimal _Price { get; set; }
}