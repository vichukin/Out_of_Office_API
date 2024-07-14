
using Out_of_Office_API.DTOs;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Out_of_Office_API.Services
{
    public interface IAuthenticationService
    {
        public Task<TokenDTO> CreateToken(string? oldRefreshToken = null);
        public Task<bool> ValidateUser(EmployeeLoginDTO employee);
        //public Task<TokenDTO> RefreshAccessToken(string refreshToken);
        public Task<bool> RegisterUser(EmployeeRegisterDTO employee);
        //public void LoginOrCreateUser(ClaimsPrincipal user);
        public EmployeeInfoDTO GetEmployeeInfo(TokenDTO token);
    }
}
