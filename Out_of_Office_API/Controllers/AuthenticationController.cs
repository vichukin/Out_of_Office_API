using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Out_of_Office_API.CustomErrors;
using Out_of_Office_API.Data;
using Out_of_Office_API.DTOs;
using Out_of_Office_API.Functions;
using System.ComponentModel;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Out_of_Office_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<Employee> manager;
        private readonly CompanyDBContext context;
        private readonly IConfiguration configuration;
        private readonly SignInManager<Employee> signInManager;
        private readonly Services.IAuthenticationService authentication;
        BlobServiceClient blob;
        BlobContainerClient container;
        public AuthenticationController(CompanyDBContext context, UserManager<Employee> manager, IConfiguration configuration, SignInManager<Employee> signInManager, Services.IAuthenticationService authentication, BlobServiceClient client)
        {
            this.manager = manager;
            this.context = context;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.authentication = authentication;
            this.blob = client;
            container = this.blob.GetBlobContainerClient("employeeimages");
            container.CreateIfNotExists();
            container.SetAccessPolicy(PublicAccessType.BlobContainer);
        }
        // GET: api/<AuthenticationController>
        [HttpGet("GetEmployeeInfo")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeInfo()
        {
            var emp = await EmployeeFunctions.GetUser(manager, User);
            if (emp == null) return Unauthorized(new Error("access token is invalid"));
            EmployeeInfoDTO empInfoDTO = new EmployeeInfoDTO()
            {
                FullName = emp.FullName,
                Id = emp.Id,
                ImagePath = emp.PhotoPath,
                SubDivision = emp.SubDivision,
                Position = emp.Position,
                Username = emp.UserName,

            };
            return Ok(empInfoDTO);

        }

        // GET api/<AuthenticationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthenticationController>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(EmployeeLoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                if (!await authentication.ValidateUser(loginDTO)) return Unauthorized(new Error("Wrong email or password"));
                var tokenDto = await authentication.CreateToken();
                var employeeInfo = authentication.GetEmployeeInfo(tokenDto);
                return Ok(employeeInfo);
            }
            return BadRequest(new Error("Required fields were not specified"));
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(EmployeeRegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(registerDTO.Photo!=null)
                        registerDTO.PhotoPath= await BlobContainerFunctions.UploadImage(container, registerDTO.Photo);
                    registerDTO.Username = GenerateUsername(registerDTO.Fullname);
                    var res = await authentication.RegisterUser(registerDTO);
                    if (res)
                    {
                        return Ok(registerDTO);
                    }
                }
                catch (ErrorException ex)
                {
                    return BadRequest(ex.GetErrors());
                }
            }
            return BadRequest(new Error("Required fields were not specified"));
        }
        private string GenerateUsername(string Fullname)
        {
            var username  = Fullname.Replace(" ", "").Substring(0, 5);
            username += context.Employees.Count();
            return username;

        }
    }
}
