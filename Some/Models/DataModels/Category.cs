using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using CourseProject.Attributes;

namespace CourseProject.Models.DataModels
{
    [TableName("Category")]
    public class Category : Entity
    {
        [DisplayName("Category name")]
        public string Name { get; set; }


    }
}