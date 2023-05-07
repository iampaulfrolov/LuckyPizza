using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models.ViewModels;

public class UserRegisterViewModel
{
    [Required] [DisplayName("Username")] public string UserName { get; set; }
    [Required] public string Name { get; set; }
    [Required] [DisplayName("Surname")] public string SurName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class UserLoginViewModel
{
    [Required] public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}