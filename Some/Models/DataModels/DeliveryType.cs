using CourseProject.Attributes;

namespace CourseProject.Models.DataModels;

[TableName("DeliveryType")]
public class DeliveryType : Entity
{
    public string Name { get; set; }
}