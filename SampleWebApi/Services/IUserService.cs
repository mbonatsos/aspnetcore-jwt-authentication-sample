﻿using System.Threading.Tasks;
using SampleWebApi.Models;

namespace SampleWebApi.Services
{
    public interface IUserService
    {
        Task<string> Login(User user, string password);
        Task<User> Register(User user, string password);
    }
}