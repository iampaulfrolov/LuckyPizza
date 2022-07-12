using CourseProject.Attributes;

namespace CourseProject.Models.DataModels
{
    [TableName("orderStatus")]
    public class OrderStatus : Entity
    {
        public string Name { get; set; }
    }
}