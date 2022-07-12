using CourseProject.Attributes;

namespace CourseProject.Models.DataModels
{
    [TableName("Delivery_type")]
    public class DeliveryType : Entity
    {
        public string Name { get; set; }
    }
}