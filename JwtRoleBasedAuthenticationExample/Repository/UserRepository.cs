using JwtRoleBasedAuthenticationExample.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace JwtRoleBasedAuthenticationExample.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public static User user = new User();


        public async Task<User>Register (User request)
        {
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.UserName; 
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Get(int id)
        {
            
            return await _context.Users.FindAsync(id);

        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.FirstName == name);
        }

        public async Task<User> ValidateLogin(string username, string password)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
            return user;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }



    }
}
