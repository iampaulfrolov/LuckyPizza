using CourseProject.Attributes;

namespace CourseProject.Models.DataModels
{
    [TableName("DeliveryProvider")]
    public class DeliveryProvider : Entity
    {
        public string Name { get; set; }
    }
}