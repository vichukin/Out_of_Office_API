using Out_of_Office_API.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Out_of_Office_API.Functions
{
    public static class EmployeeFunctions
    {
        public static async Task<Employee> GetUser(UserManager<Employee> userManager, ClaimsPrincipal User)
        {
            return await userManager.FindByNameAsync(User.Identity.Name);
        }
    }
}
