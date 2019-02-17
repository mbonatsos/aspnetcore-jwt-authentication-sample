using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleWebApi.Data;
using SampleWebApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public UserService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Returns an access token uppon successful user login
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>the access token</returns>
        public async Task<string> Login(User user, string password)
        {
            var authenticatedUser = await Authenticate(user, password);

            if (authenticatedUser == null)
                return null;

            var accessToken = GenerateToken(authenticatedUser);
            return accessToken;
        }

        /// <summary>
        /// Registers a <see cref="User"/>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> Register(User user, string password)
        {
            var userExists = await _userRepository.UserExistsAsync(user);

            if (userExists)
                return null;

            // hash password
            var hashed = CreatePasswordHash(password);
            user.PasswordHash = hashed.passwordHash;
            user.PasswordSalt = hashed.passwordSalt;

            var createdUser = await _userRepository.CreateUserAsync(user, password);
            return createdUser;
        }

        /// <summary>
        /// Authenticates the given user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>The authenticated user</returns>
        private async Task<User> Authenticate(User user, string password)
        {
            var storedUser = await _userRepository.GetUserByEmailAsync(user.Email);

            if (storedUser == null)
                return null;

            var passwordVerified = VerifyPasswordHash(storedUser, password);

            if (!passwordVerified)
                return null;

            return storedUser;
        }

        private bool VerifyPasswordHash(User user, string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        private (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return (passwordHash, passwordSalt);
            }
        }

        private string GenerateToken(User user)
        {
            // create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            // generate token
            var secretKey = _configuration["Settings:SigningKey"];
            var encodedKey = Encoding.ASCII.GetBytes(secretKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(encodedKey);
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            var tokenString = jwtSecurityTokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
