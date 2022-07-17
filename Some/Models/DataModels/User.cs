using System.ComponentModel.DataAnnotations.Schema;
using CourseProject.Attributes;
using CourseProject.Identity.Models;

namespace CourseProject.Models.DataModels

{
    [TableName("User_")]
    public class User:Entity
    {
        public string UserName { get; set; }
        public string Surname { get; set; }

        public string Name  { get; set; }
        public string PasswordHash { get; set; }

        [ForeignKey("role_id")] 
        public Role Role { get; set; }
        public string PhoneNumber { get; set; }

        public User()
        {
            Role = new Role();
        }
    }
}