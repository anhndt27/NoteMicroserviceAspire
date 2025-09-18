
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
using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Auths;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Implement.Service
{
    public class AuthenticationsAsyncService : IAuthenticationsAsyncService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        
        public AuthenticationsAsyncService(IConfiguration configuration, ApplicationDbContext dbContext, JwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserGroups).ThenInclude(ug => ug.Group)
                .FirstOrDefaultAsync(u => u.UserName == request.UserName);
            var groupIds = await _userRepository.GetUserGroupIdsAsync(user.Id);
                
            if (VerifyPassword(request.Password, user.Password))
            {
                var token = _jwtTokenGenerator.GenerateToken(user.Id, groupIds);
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
                Token = ""
            };
        }
        
        public async Task<string> Register(RegisterRequestDto request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName == request.Username || u.Email == request.Email))
            {
                return "Username hoặc Email đã tồn tại";
            }

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
            };

            user.Password = HashPassword(request.Password);

            _dbContext.Users.Add(user);
            var result = await _dbContext.SaveChangesAsync();

            return result > 0 ? "Success" : "Fail";
        }
        
        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
    
            int iterations = 10000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, hashAlgorithm);
            byte[] hash = pbkdf2.GetBytes(32);
            return $"{Convert.ToBase64String(salt)}:{iterations}:{Convert.ToBase64String(hash)}";
        }
        
        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                var parts = hashedPassword.Split(':');
                if (parts.Length != 3) return false;

                var salt = Convert.FromBase64String(parts[0]);
                var iterations = int.Parse(parts[1]);
                var storedHash = Convert.FromBase64String(parts[2]);

                HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, hashAlgorithm);
                var hashAttempt = pbkdf2.GetBytes(32);

                return CryptographicOperations.FixedTimeEquals(hashAttempt, storedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}
