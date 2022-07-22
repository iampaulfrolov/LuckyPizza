using System.Threading.Tasks;
using CourseProject.Models.DataModels;

namespace CourseProject.Data.Repositories;

public interface IUserRepository : IRepository<User>
{
    public Task<User> FindByLogin(string loginProvider, string providerKey);

}