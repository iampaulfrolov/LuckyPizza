using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CourseProject.Models.DataModels;
using Microsoft.AspNetCore.Http;

namespace CourseProject.Models.ViewModels
{
    public class ProductCreateViewModel
    {
        public Product Product { get; set; }
        public IFormFile Image { get; set; }
        public List<Category> Categories { get; set; }

        public ProductCreateViewModel()
        {
            Product = new Product();
            Categories = new List<Category>();
        }
    }

    public class ProductUpdateViewModel : ProductCreateViewModel
    {
        public ProductUpdateViewModel() : base()
        {
        }
    }


    public class ProductIndexViewModel
    {
        public List<Product> Products { get; set; }
        public List<CategoryViewModel> Categories { get; }

        public ProductIndexViewModel(IEnumerable<Category> flatCategories)
        {
            var categories = (from fc in flatCategories
                select new CategoryViewModel()
                {
                    Id = fc.Id,
                    Name = fc.Name
                }).ToList();


            Categories = categories;
        }
    }
}