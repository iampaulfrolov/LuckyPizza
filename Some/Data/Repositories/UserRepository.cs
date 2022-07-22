using System.Linq;
using System.Threading.Tasks;
using CourseProject.Models;
using CourseProject.Models.DataModels;
using Microsoft.Extensions.Options;

namespace CourseProject.Data.Repositories;

public class UserRepository : AdoNetRepository<User>, IUserRepository
{
    public UserRepository(IOptions<Settings> option) : base(option)
    {
    }

    public async Task<User> FindByLogin(string loginProvider, string providerKey)
    {
        var query =
            @$"SELECT TOP 1 LoginProvider, ProviderKey, Email, UserName, Surname, Name, PasswordHash, role_id, PhoneNumber, Id FROM USER_ WHERE LoginProvider='{loginProvider}' AND ProviderKey='{providerKey}'";
        var user = await ExecuteSelect(query);
        return user.FirstOrDefault();
    }
}