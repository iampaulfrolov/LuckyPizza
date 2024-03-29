﻿using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CourseProject.Data;
using CourseProject.Data.Repositories;
using CourseProject.Models.DataModels;
using CourseProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers;

[Authorize(Roles = RoleConst.All)]
public class OrderController : Controller
{
    private readonly IRepository<DeliveryProvider> _deliveryProviderRepository;
    private readonly IRepository<Delivery> _deliveryRepository;
    private readonly IRepository<DeliveryType> _deliveryTypeRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<OrderStatus> _statusRepository;
    private readonly UserManager<User> _userManager;

    public OrderController(UserManager<User> userManager,
        IRepository<Order> orderRepository,
        IRepository<Delivery> deliveryRepository,
        IRepository<DeliveryType> deliveryTypeRepository,
        IRepository<DeliveryProvider> deliveryProviderRepository, IRepository<OrderStatus> statusRepository)
    {
        _userManager = userManager;
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
        _deliveryTypeRepository = deliveryTypeRepository;
        _deliveryProviderRepository = deliveryProviderRepository;
        _statusRepository = statusRepository;
    }

    public async Task<IActionResult> Index(int id)
    {
        var orders = await _orderRepository.GetMany(order => order.User.Id, id);
        return View(orders);
    }


    public async Task<IActionResult> Create()
    {
        if (GetCart().Items.Count == 0)
            return RedirectToAction("Index", "Cart");
        var model = new OrderCreateViewModel
        {
            Delivery = new Delivery { Date = DateTime.Now },
            DeliveryProviders = (await _deliveryProviderRepository.GetAll()).ToList(),
            DeliveryTypes = (await _deliveryTypeRepository.GetAll()).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderCreateViewModel model)
    {
        if (!ModelState.IsValid) return RedirectToAction("Create");
        var order = new Order
        {
            Status = { Id = 1 },
            User = { Id = int.Parse(_userManager.GetUserId(User)) },
            Date = DateTime.Now
        };
        var data = GetCart();
        if (data.Items.Count == 0) return RedirectToAction("Index", "Cart");
        foreach (var item in data.Items)
            order.Products.Add(new OrderProduct(item.Quantity, item.Quantity * item.Product.Price)
                { Id = item.Product.Id });

        var delivery = new Delivery
        {
            Date = DateTime.Now,
            DeliveryProvider = model.Delivery.DeliveryProvider,
            DeliveryType = model.Delivery.DeliveryType,
            Address = model.Delivery.Address,
        };

        var orderId = await _orderRepository.Create(order);
        delivery.Order = new Order { Id = orderId };
        await _deliveryRepository.Create(delivery);
        RemoveCart();
        return RedirectToAction("Index", "User");
    }

    [Authorize(Roles = RoleConst.Admin)]
    public async Task<IActionResult> ConfirmIndex(int id = 1)
    {
        var model = new OrderConfirmViewModel
        {
            Orders = (await _orderRepository.GetMany(order => order.Status.Id, id)).ToList(),
            StatusList = (await _statusRepository.GetAll()).ToList()
        };
        return View("Confirm", model);
    }

    [Authorize(Roles = RoleConst.Admin)]
    public async Task<IActionResult> Confirm(int id)
    {
        var order = await _orderRepository.GetById(id);
        order.Status.Id = 2;
        await _orderRepository.Update(order, o => o.Id, id);
        return RedirectToAction("ConfirmIndex");
    }

    [Authorize(Roles = RoleConst.Admin)]
    public async Task<IActionResult> Reject(int id)
    {
        var order = await _orderRepository.GetById(id);
        order.Status.Id = 3;
        await _orderRepository.Update(order, o => o.Id, id);
        return RedirectToAction("ConfirmIndex");
    }

    public CartViewModel GetCart()
    {
        var savedData = Request.Cookies["cart"];
        CartViewModel data;
        if (savedData != null && savedData.Any())
            data = JsonSerializer.Deserialize<CartViewModel>(savedData);
        else
            data = new CartViewModel();
        return data;
    }

    public void RemoveCart()
    {
        Response.Cookies.Delete("cart");
    }
}