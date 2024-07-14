using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Out_of_Office_API.CustomErrors;
using Out_of_Office_API.Data;
using Out_of_Office_API.DTOs;
using Out_of_Office_API.Functions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Out_of_Office_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly CompanyDBContext context;
        private readonly UserManager<Employee> manager;
        private readonly IMapper mapper;
        private readonly BlobServiceClient blob;
        private readonly BlobContainerClient container;
        

        public EmployeeController(CompanyDBContext context, UserManager<Employee> manager, IMapper mapper, BlobServiceClient blob)
        {
            this.context = context;
            this.manager = manager;
            this.mapper = mapper;
            this.blob = blob;
            container = this.blob.GetBlobContainerClient("employeeimages");
            container.CreateIfNotExists();
            container.SetAccessPolicy(PublicAccessType.BlobContainer);
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var emp = await EmployeeFunctions.GetUser(manager, User);
            if (emp == null) return Unauthorized();
            if(emp.Position != Position.Employee)
            {
                var employees = await context.Employees.Include(t => t.LeaveRequests).ThenInclude(t=>t.ApprovalRequest).Include(t => t.Projects).ThenInclude(t=>t.ProjectManager).Include(t => t.PeoplePartner).Select(t => mapper.Map<EmployeeDTO>(t)).ToListAsync();
                return Ok(employees);
            }
            return BadRequest(new Error("You dont have permission"));
        }
        [HttpGet("GetProjectManagers")]
        public async Task<IActionResult> GetProjectManagers()
        {
            var projManagers = await context.Employees.Where(t=>t.Position==Position.Project_Manager).Select(t=>mapper.Map<EmployeeDTO>(t)).ToListAsync();
            return Ok(projManagers);
        }
        [HttpGet("GetMembers")]
        public async Task<IActionResult> GetMembers()
        {
            var projManagers = await context.Employees.Where(t => t.Position == Position.Employee).Select(t => mapper.Map<EmployeeDTO>(t)).ToListAsync();
            return Ok(projManagers);
        }
        [HttpGet("GetHrManagers")]
        public async Task<IActionResult> GetHrManagers()
        {
            var hrManagers = await context.Employees.Where(t => t.Position == Position.HR_Manager).Select(t => mapper.Map<EmployeeDTO>(t)).ToListAsync();
            return Ok(hrManagers);
        }
        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var emp = await context.Employees.Include(t=>t.PeoplePartner).FirstOrDefaultAsync(t=>t.Id==id);
            if (emp == null) return NotFound();
            var empDTO = mapper.Map<EmployeeDTO>(emp);

            return Ok(empDTO);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, EmployeeEditDTO dto)
        {
            var emp = await context.Employees.FirstOrDefaultAsync(t=>t.Id==id);
            if (emp == null) return NotFound();
            emp.FullName = dto.Fullname;
            emp.OutOfOfficeBalance = dto.OutOfOfficeBalance;
            emp.SubDivision = dto.SubDivision;
            emp.Position = dto.Position;
            emp.PeoplePartnerId = dto.PeoplePartnerId;
            if(dto.Photo != null)
            {
                if(emp.PhotoPath !=null)
                    BlobContainerFunctions.DeleteImage(container, emp.PhotoPath);
                emp.PhotoPath = await BlobContainerFunctions.UploadImage(container, dto.Photo);
            }
            context.Employees.Update(emp);
            await context.SaveChangesAsync();
            return Ok();

        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
