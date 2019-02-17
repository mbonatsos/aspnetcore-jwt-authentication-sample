using System.Threading.Tasks;
using SampleWebApi.Models;

namespace SampleWebApi.Data
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}