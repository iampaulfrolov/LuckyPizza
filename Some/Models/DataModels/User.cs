using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using CourseProject.Attributes;
using CourseProject.Identity.Models;

namespace CourseProject.Models.DataModels;

[TableName("User_")]
public class User : Entity
{
    public User()
    {
        Role = new Role();
    }

    public string LoginProvider { get; set; }
    public string ProviderKey { get; set; }
    public string Email { get; set; }
    [DisplayName("UserName")] public string UserName { get; set; }
    public string Surname { get; set; }

    public string Name { get; set; }
    public string PasswordHash { get; set; }

    [ForeignKey("role_id")] public Role Role { get; set; }

    public string PhoneNumber { get; set; }
}