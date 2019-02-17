using Microsoft.EntityFrameworkCore;
using SampleWebApi.Models;
using System.Threading.Tasks;

namespace SampleWebApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateUserAsync(User user)
        {
            await _dataContext.AddAsync(user);
            await _dataContext.SaveChangesAsync();
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _dataContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
