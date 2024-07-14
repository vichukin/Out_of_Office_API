using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Out_of_Office_API.Data;
using Out_of_Office_API.DTOs;
using Out_of_Office_API.Functions;

namespace Out_of_Office_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly CompanyDBContext _context;
        private readonly IMapper mapper;
        private readonly UserManager<Employee> manager;

        public ProjectsController(CompanyDBContext context,IMapper mapper, UserManager<Employee> manager)
        {
            _context = context;
            this.mapper = mapper;
            this.manager = manager;
        }

        // GET: api/Projects
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProjects()
        {
            var emp = await EmployeeFunctions.GetUser(manager, User);
            if(emp== null) return Unauthorized();
            var projects = await _context.Projects.Include(t=>t.ProjectManager).Include(t=>t.Members).Select(t=>mapper.Map<ProjectDTO>(t)).ToListAsync();
            return Ok(projects );
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _context.Projects.Include(t => t.ProjectManager).Include(t=>t.Members).FirstOrDefaultAsync(t => t.Id == id);

            if (project == null)
            {
                return NotFound();
            }
            var projDTO = mapper.Map<ProjectDTO>(project);
            return Ok(projDTO);
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectCreateDTO project)
        {
            var proj = await _context.Projects.Include(t=>t.Members).FirstOrDefaultAsync(t=>t.Id ==id);
            if (proj == null) return NotFound();
            proj.StartDate = project.StartDate;
            proj.ProjectType = project.ProjectType;
            proj.ProjectManagerId = project.ProjectManagerId;
            proj.Comment = project.Comment;
            if(project.MemberIds!=null)
            {
                List<Employee> members = new List<Employee>();
                foreach (var member in project.MemberIds)
                {
                    var memb = await _context.Employees.FirstOrDefaultAsync(t => t.Id == member);
                    members.Add(memb);
                }
                proj.Members = members;
            }
            _context.Update(proj);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Project>> PostProject(ProjectCreateDTO project)
        {
            if(ModelState.IsValid)
            {
                
                var proj = new Project()
                {
                    ProjectManagerId = project.ProjectManagerId,
                    ProjectType = project.ProjectType,
                    ProjectStatus = ProjectStatus.Active,
                    Comment = project.Comment,
                    StartDate = project.StartDate,
                    
                };
                if (project.MemberIds != null)
                {
                    List<Employee> members = new List<Employee>();
                    foreach (var member in project.MemberIds)
                    {
                        var memb = await _context.Employees.FirstOrDefaultAsync(t=>t.Id==member);
                        members.Add(memb);
                    }
                    proj.Members = members;
                }
                _context.Projects.Add(proj);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Required fields were not specified");
        }

        [HttpPatch("Active/{id}")]
        public async Task<IActionResult> ActiveProject(int id)
        {
            var proj = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (proj == null) return NotFound();
            proj.ProjectStatus = ProjectStatus.Active;
            proj.EndDate = null;
            _context.Projects.Update(proj);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPatch("Inactive/{id}")]
        public async Task<IActionResult> InactiveProject(int id)
        {
            var proj = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (proj == null) return NotFound();
            proj.ProjectStatus = ProjectStatus.Inactive;
            proj.EndDate = DateOnly.Parse(DateTime.Now.ToShortDateString());
            _context.Projects.Update(proj);
            await _context.SaveChangesAsync();
            return Ok();
        }
        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
