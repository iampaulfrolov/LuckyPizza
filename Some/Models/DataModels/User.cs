using System.ComponentModel.DataAnnotations.Schema;
using CourseProject.Attributes;
using CourseProject.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Models.DataModels

{
    [TableName("User_")]
    public class User:Entity
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string Email { get; set; }
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