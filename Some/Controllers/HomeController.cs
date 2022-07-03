using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CourseProject.Data.Repositories;
using CourseProject.Identity.Models;
using CourseProject.Models;
using CourseProject.Models.DataModels;
using CourseProject.Models.ViewModels;
using Kursach.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ErrorViewModel = CourseProject.Models.ErrorViewModel;

namespace CourseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Product> _repository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Order> _orderRepository;

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, IRepository<Product> repository,
            SignInManager<User> signInManager, UserManager<User> userManager, IRepository<Role> role,
             IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
            _roleRepository = role;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _repository = repository;
        }


        public async Task<IActionResult> Index()
        {
            //var roles = await _roleRepository.GetAll();

            var user = new User() {Id = 24, Name = "gordon",UserName = "informatyka4444@wp.pl"};
            //var result = await _userManager.CreateAsync(user, "password#4D");
            //await _userManager.AddToRoleAsync(user,"admin");
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
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var savedData = HttpContext.Session.GetString("cart");
            var data = new CartViewModel();
            if ((savedData != null) && (savedData.Any()))
            {
                data = JsonConvert.DeserializeObject<CartViewModel>(savedData);
            }

            var product = await _repository.GetById(id);
            if (data.Items.Any(p => p.Product.Id == product.Id))
                data.Items.Where(i => i.Product.Id == product.Id).ToList().ForEach(i => i.Quantity++);
            else
            {
                data.AddItem(new CartItemViewModel(product));
            }

            var stringData = JsonConvert.SerializeObject(data, Formatting.Indented);
            HttpContext.Session.SetString("cart", stringData);
            return RedirectToAction("Index");
        }
    }
}