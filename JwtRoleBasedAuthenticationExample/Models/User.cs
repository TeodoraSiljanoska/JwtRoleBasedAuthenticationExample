using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JwtRoleBasedAuthenticationExample.Models
{
    public class User
    {
        [JsonIgnore]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; }
        [MinLength(8)]
        public string Password { get; set; } 
        [Required,JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [Required,JsonIgnore]
        public byte[] PasswordSalt { get; set; }
    }
}
