using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourseProject.Attributes;

namespace CourseProject.Models.DataModels;

[TableName("Product")]
public class Product : Entity
{
    public Product()
    {
        Category = new Category();
    }

    [Required] [DisplayName("Name")] public string Title { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Description")]
    public string Description { get; set; }

    [DataType(DataType.Currency)]
    [DisplayName("Price")]
    public decimal Price { get; set; }

    [ForeignKey("category_id")] public Category Category { get; set; }

    public int Quantity { get; set; }

    public string Image { get; set; }
}