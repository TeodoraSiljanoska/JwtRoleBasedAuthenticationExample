using JwtRoleBasedAuthenticationExample.Models;

namespace JwtRoleBasedAuthenticationExample.Repository
{
    public interface IUserRepository
    {
        Task<User> Register(User user);
        Task<User> Get(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByName(string name);
        Task<User> ValidateLogin(string username, string password);
    }
}
