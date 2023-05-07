using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CourseProject.Data.Repositories;
using CourseProject.Models;
using CourseProject.Models.DataModels;
using CourseProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourseProject.Controllers;

public class HomeController : Controller
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _repository;


    public HomeController(IRepository<Product> repository,
        IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
        _repository = repository;
    }


    public async Task<IActionResult> Index()
    {
        var products = await _repository.GetAll();
        var t = await _repository.Get(product => product.Category.Id, 1);
        var m = await _repository.Get(product => product.Price, 124);
        var order = await _orderRepository.GetAll();
        return View(products);
    }

    public async Task<IActionResult> SortBy()
    {
        var products = await _repository.GetAll();
        products = products.OrderBy(prod => prod.Title);
        return View("Index", products);
    }

    [Authorize(Roles = "anon")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> AddToCart(int id)
    {
        var savedData = HttpContext.Session.GetString("cart");
        var data = new CartViewModel();
        if (savedData != null && savedData.Any()) data = JsonConvert.DeserializeObject<CartViewModel>(savedData);

        var product = await _repository.GetById(id);
        if (data.Items.Any(p => p.Product.Id == product.Id))
            data.Items.Where(i => i.Product.Id == product.Id).ToList().ForEach(i => i.Quantity++);
        else
            data.AddItem(new CartItemViewModel(product));

        var stringData = JsonConvert.SerializeObject(data, Formatting.Indented);
        HttpContext.Session.SetString("cart", stringData);
        return RedirectToAction("Index");
    }
}