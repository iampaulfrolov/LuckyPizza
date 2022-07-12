using CourseProject.Attributes;

namespace CourseProject.Models.DataModels
{
    [TableName("Provider")]
    public class DeliveryProvider : Entity
    {
        public string Name { get; set; }
    }
}