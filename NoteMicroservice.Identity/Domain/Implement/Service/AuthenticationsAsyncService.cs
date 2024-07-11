
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteMicroservice.Identity.Domain.Implement.Service
{
    public class AuthenticationsAsyncService : IAuthenticationsAsyncService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationsAsyncService(IConfiguration configuration,UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<LoginResponseViewModel> Login(LoginRequestViewModel request)
        {
            var user = await _userManager.Users.Include(u => u.Group).FirstOrDefaultAsync(u => u.UserName == request.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var token = GenerateJwtToken(user.UserName);
                return new LoginResponseViewModel()
                {
                    UserId = user.Id,
                    UserName = request.Username,
                    Token = token,
                    GroupId = user.GroupId,
                    GroupName = user.Group?.Name
                };
            }
            return new LoginResponseViewModel()
            {
                UserName = request.Username,
                Token = "Fail"
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> Register(RegisterRequestViewModel request)
        {
            var user = new User { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            if(result.Succeeded)
            {
                return "Success";
            }
            return "Fail";
        }


        private string GenerateJwtToken(string userName)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

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
