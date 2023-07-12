using JwtRoleBasedAuthenticationExample.Models;
using JwtRoleBasedAuthenticationExample.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtRoleBasedAuthenticationExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //public static User user = new User();

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            
        }
        public static User user = new User();
        
       

        [HttpPost("user/register")]
        public async Task<ActionResult<User>> Register(User request)
        {
            /* var createdUser = await _userRepository.AddUser(user)
             user.FirstName = request.FirstName;
             user.LastName = request.LastName;
             user.Email = request.Email;
             user.UserName = request.UserName;
             CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
             user.PasswordHash = passwordHash;
             user.PasswordSalt = passwordSalt;
             var createdUser = await _userRepository.AddUser(user);*/
            var createdUser = await _userRepository.Register(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);

            //return Ok(user);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {

            return await _userRepository.Get(id);
        }

        [HttpGet("user/Login")]
        public async Task<ActionResult<User>> Login(string username, string password)
        {
            var existingUser = await _userRepository.ValidateLogin(username, password);
            if (existingUser == null)
            {
                return NotFound("Invalid username or password");
            }
            return Ok(existingUser);
        }

       /* [HttpPost("login")]
        public async Task<ActionResult<string>> Login(User request)
        {
            if (user.UserName != request.UserName)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);
            return Ok(token);
        } */

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "User")
            };
            var Key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}