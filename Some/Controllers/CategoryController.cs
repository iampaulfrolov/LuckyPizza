using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProject.Data.Repositories;
using CourseProject.Models.DataModels;
using CourseProject.Models.ViewModels;

namespace CourseProject.Controllers
{
    public class CategoryController : Controller
    {

        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Create()
        {
            return View(new CategoryCreateViewModel(){ Category = new CategoryViewModel()});
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            var category = new Category() {Name = model.Category.Name};
            await _categoryRepository.Create(category);
            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cat = await _categoryRepository.GetById(id);
            return View(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            await _categoryRepository.Update(category, cat => cat.Id, category.Id);
            return RedirectToAction("Index", "Product");
        }
    }
}
