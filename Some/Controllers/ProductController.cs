using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CourseProject.Data;
using CourseProject.Data.Repositories;
using CourseProject.Models.DataModels;
using CourseProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers;

[AllowAnonymous]
public class ProductController : Controller
{
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly UserManager<User> _userManager;

    public ProductController(IProductRepository productRepository, IRepository<Category> categoryRepository,
        IWebHostEnvironment appEnvironment, UserManager<User> userManager)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _appEnvironment = appEnvironment;
        _userManager = userManager;
    }


    public async Task<IActionResult> Index(string sortOrder = "price_normal")
    {
        var products = await _productRepository.GetAll();
        var categories = await _categoryRepository.GetAll();
        products = sortOrder switch
        {
            "price_desc" => products.OrderByDescending(s => s.Price),
            "price_asc" => products.OrderBy(s => s.Price),
            "price_normal" => products,
            _ => products
        };

        var model = new ProductIndexViewModel(categories)
        {
            Products = products.ToList()
        };
        return View(model);
    }

    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var userId = int.Parse(_userManager.GetUserId(User));
        var products = await _productRepository.GetByCategory(categoryId, userId);
        var categories = await _categoryRepository.GetAll();
        var model = new ProductIndexViewModel(categories)
        {
            Products = products.ToList()
        };
        return View("Index", model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productRepository.GetById(id);
        return View(product);
    }

    [Authorize(Roles = RoleConst.Admin)]
    public async Task<IActionResult> Create()
    {
        var model =
            new ProductCreateViewModel
            {
                Categories = (await _categoryRepository.GetAll()).ToList()
            };
        return View(model);
    }

    [Authorize(Roles = RoleConst.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        var createdProduct = model.Product;
        if (model.Image != null)
        {
            var path = "/Images/" + model.Image.FileName;
            await using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);
            await model.Image.CopyToAsync(fileStream);
            fileStream.Close();
            createdProduct.Image = path;
        }


        await _productRepository.Create(createdProduct);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = RoleConst.Admin)]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productRepository.GetById(id);
        var categories = await _categoryRepository.GetAll();
        return View(new ProductUpdateViewModel { Product = product, Categories = categories.ToList() });
    }

    [Authorize(Roles = RoleConst.Admin)]
    [HttpPost]
    public async Task<IActionResult> Edit(ProductUpdateViewModel model)
    {
        if (model.Image != null)
        {
            var path = "/Images/" + model.Image.FileName;
            await using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);
            await model.Image.CopyToAsync(fileStream);
            fileStream.Close();
            model.Product.Image = path;
        }

        await _productRepository.Update(model.Product, x => x.Id, model.Product.Id);
        return RedirectToAction("Details", model.Product);
    }
}