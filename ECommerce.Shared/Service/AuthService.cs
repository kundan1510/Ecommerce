using ECommerce.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Shared.Service
{
    public class AuthService
    {
        private readonly JWTSettings _jwtSettings;
        private List<User> _userList;



        public AuthService(JWTSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
            _userList = new List<User>()
            {
                new User { Email = "kk@gmail.com", HashedPassword= "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", Role = "Admin" },
                new User { Email = "kk1@gmail.com", HashedPassword = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", Role =  "Admin" },
                new User { Email = "kk2@gmail.com", HashedPassword = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", Role = "Admin" }
            };
        }

        public string GenerateJwtToken(User user, IEnumerable<Claim> additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Role", user.Role),
            };

            if (additionalClaims != null)
            {
                claims.AddRange(additionalClaims);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool RegisterUser(string email, string password)
        {
            User userAdded = new User();
            userAdded.Email = email;
            userAdded.HashedPassword = HashPassword(password);
            userAdded.Role = "User";
            _userList.Add(userAdded);
            return true;
        }

        // Retrieve a user by PartitionKey ("USER") and RowKey (email)
        public User GetUserAsync(string email)
        {
            return this._userList.FirstOrDefault(e => e.Email == email);
        }

        public bool ValidateUser(string email, string password)
        {
            var hashedEnteredPassword = HashPassword(password);
            var user = _userList.FirstOrDefault(e => e.Email == email);
            return hashedEnteredPassword == user.HashedPassword;
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
