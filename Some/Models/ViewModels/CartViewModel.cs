using System;
using System.Collections.Generic;
using CourseProject.Models.DataModels;

namespace CourseProject.Models.ViewModels;

[Serializable]
public class CartViewModel
{
    public CartViewModel(List<CartItemViewModel> items)
    {
        Items = items;
    }

    public CartViewModel()
    {
        Items = new List<CartItemViewModel>();
    }

    public decimal Amount { get; set; }
    public List<CartItemViewModel> Items { get; set; }

    public void AddItem(CartItemViewModel item)
    {
        Items.Add(item);
        Amount += item.Quantity * item.Product.Price;
    }
}

[Serializable]
public class CartItemViewModel
{
    public CartItemViewModel(Product product)
    {
        Product = product;
        Quantity = 1;
    }

    public Product Product { get; set; }

    public int Quantity { get; set; }
}