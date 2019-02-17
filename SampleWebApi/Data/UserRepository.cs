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

        public async Task<User> CreateUserAsync(User user, string password)
        {
            var entityEntry = await _dataContext.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _dataContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<bool> UserExistsAsync(User user)
        {
            return _dataContext.Users.AnyAsync(x => x.Email == user.Email || x.Username == user.Username);
        }
    }
}
