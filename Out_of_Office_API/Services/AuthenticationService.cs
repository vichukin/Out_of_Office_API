
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Headers;
using Out_of_Office_API.Data;
using Out_of_Office_API.DTOs;
using Out_of_Office_API.CustomErrors;

namespace Out_of_Office_API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<Employee> userManager;
        private readonly IConfiguration configuration;
        private readonly CompanyDBContext context;
        private Employee? _employee;    

        public AuthenticationService(UserManager<Employee> userManager, IConfiguration configuration, CompanyDBContext context)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.context = context;
        }

        public async Task<TokenDTO> CreateToken(string? oldToken = null)
        {
            var claims = await GetClaims();
            var signinCredential = GetSigningCredentials();
            var tokenOptions = GenerateTokenOptions(signinCredential, claims);
            TokenDTO token = new TokenDTO()
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions)
            };
            return token;
        }
        public EmployeeInfoDTO GetEmployeeInfo(TokenDTO token)
        {
            EmployeeInfoDTO employeeInfoDTO = new EmployeeInfoDTO()
            {
                Id = _employee.Id,
                ImagePath = _employee.PhotoPath,
                SubDivision = _employee.SubDivision,
                FullName = _employee.FullName,
                Position = _employee.Position,
                Username = _employee.UserName,
                AccessToken = token.accessToken
            };
            return employeeInfoDTO;
        }
        public async Task<bool> ValidateUser(EmployeeLoginDTO employee)
        {
            var us = await userManager.FindByNameAsync(employee.Username);
            if(us == null) return false;
            var res= await userManager.CheckPasswordAsync(us, employee.Password);
            if (res)
                _employee = us;
            return res;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // Срок действия токена (1 день)
                signingCredentials: signingCredentials
            );
            return token;
        }
        private async Task<List<Claim>> GetClaims()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "Employee"),
                new Claim(ClaimTypes.NameIdentifier, _employee.Id),
                new Claim(ClaimTypes.Name, _employee.UserName)
            };
            if (await userManager.IsInRoleAsync(_employee, "Admin"))
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            return claims;
        }
        private SigningCredentials GetSigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return signin;
        }

        public async Task<bool> RegisterUser(EmployeeRegisterDTO employee)
        {
            var createdUser = new Employee() { UserName = employee.Username, Position=employee.Position,FullName=employee.Fullname, SubDivision=employee.SubDivision ,PeoplePartnerId = employee.PeoplePartnerId,OutOfOfficeBalance = employee.OutOfOfficeBalance, PhotoPath = employee.PhotoPath};
            var check = await userManager.FindByNameAsync(createdUser.UserName);
            if (check != null)
            {
                throw new ErrorException("This username already taken");
            }
            var res = await userManager.CreateAsync(createdUser, employee.Password);
            if (res.Succeeded)
            {
                createdUser = await userManager.FindByNameAsync(employee.Username);
                _employee = createdUser;
                return true;
            }
            else
            {
                throw new ErrorException(res.Errors.ToList()); 
            }
        }

        //public async void LoginOrCreateUser(ClaimsPrincipal user)
        //{
        //    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var existingUser = await userManager.FindByIdAsync(userId);
        //    if (existingUser == null)
        //    {
        //        existingUser = new Employee
        //        {
        //            Id = userId,
        //            // Дополнительные данные о пользователе
        //            FirstName = user.FindFirst(ClaimTypes.GivenName)?.Value,
        //            Surname = user.FindFirst(ClaimTypes.Surname)?.Value,
        //            Email = user.FindFirst(ClaimTypes.Email)?.Value,
        //            ImagePath = user.FindFirst("picture")?.Value,
        //            // Другие поля, которые вы хотите сохранить
        //        };
        //        var res = await userManager.CreateAsync(existingUser);
        //        if (res.Succeeded)
        //        {
        //            existingUser = await userManager.FindByNameAsync(existingUser.Email);
        //            _user = existingUser;
        //        }
        //        else
        //        {
        //            throw new ErrorException(res.Errors.ToList());
        //        }
        //    }
        //    else
        //        _user = existingUser;
        //}
    }
}
