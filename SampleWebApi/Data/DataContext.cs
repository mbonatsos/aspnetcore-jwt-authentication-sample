using Microsoft.EntityFrameworkCore;
using SampleWebApi.Models;

namespace SampleWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
