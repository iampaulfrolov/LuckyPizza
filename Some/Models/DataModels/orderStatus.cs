using CourseProject.Attributes;

namespace CourseProject.Models.DataModels;

[TableName("OrderStatus")]
public class OrderStatus : Entity
{
    public string Name { get; set; }
}