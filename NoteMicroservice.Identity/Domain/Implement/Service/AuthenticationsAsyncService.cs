
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Implement.Service
{
    public class AuthenticationsAsyncService : IAuthenticationsAsyncService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        public AuthenticationsAsyncService(IConfiguration configuration,UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext dbContext)
        {
            _signInManager = signInManager;
            _dbContext = dbContext;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserGroups).ThenInclude(ug => ug.Group)
                .FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user != null && VerifyPassword(request.Password, user.Password))
            {
                var token = GenerateJwtToken(user.UserName);
                return new LoginResponseDto()
                {
                    UserId = user.Id,
                    UserName = request.UserName,
                    Token = token,
                    GroupIds = user.UserGroups?.Select(ug => ug.Group?.Id).ToList(),
                    GroupNames = user.UserGroups?.Select(ug => ug.Group?.Name).ToList()
                };
            }

            return new LoginResponseDto()
            {
                UserName = request.UserName,
                Token = "Fail"
            };
        }
        
        public async Task<string> Register(RegisterRequestDto request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName == request.Username || u.Email == request.Email))
            {
                return "Username hoặc Email đã tồn tại";
            }

            var user = new User { UserName = request.Username, Email = request.Email };
            user.Password = HashPassword(request.Password);

            _dbContext.Users.Add(user);
            var result = await _dbContext.SaveChangesAsync();

            return result > 0 ? "Success" : "Fail";
        }
        
        private string HashPassword(string password)
        {
            const int saltSize = 16;
            const int iterations = 10000;
            var salt = RandomNumberGenerator.GetBytes(saltSize);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var key = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(salt) + ":" + iterations + ":" + Convert.ToBase64String(key);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                var parts = hashedPassword.Split(':');
                var salt = Convert.FromBase64String(parts[0]);
                var iterations = int.Parse(parts[1]);
                var key = Convert.FromBase64String(parts[2]);
                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
                var keyToCheck = pbkdf2.GetBytes(32);
                return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
            }
            catch { return false; }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        private string GenerateJwtToken(string userName, List<string> groupIds = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (groupIds != null && groupIds.Any())
            {
                claims.AddRange(groupIds.Select(g => new Claim("group", g)));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
