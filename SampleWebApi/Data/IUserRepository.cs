﻿using System.Threading.Tasks;
using SampleWebApi.Models;

namespace SampleWebApi.Data
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(User user);
    }
}