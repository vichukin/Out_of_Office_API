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
using Out_of_Office_API.CustomErrors;
using Out_of_Office_API.Data;
using Out_of_Office_API.DTOs;
using Out_of_Office_API.Functions;

namespace Out_of_Office_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalRequestsController : ControllerBase
    {
        private readonly CompanyDBContext _context;
        private readonly IMapper mapper;
        private readonly UserManager<Employee> manager;

        public ApprovalRequestsController(CompanyDBContext context, IMapper mapper, UserManager<Employee> manager)
        {
            _context = context;
            this.mapper = mapper;
            this.manager = manager;
        }

        // GET: api/ApprovalRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalRequest>>> GetApprovalRequests()
        {
            var approvalRequests = await _context.ApprovalRequests.Include(t => t.LeaveRequest).Include(t => t.Approver).Include(t=>t.LeaveRequest.Employee).Select(t => mapper.Map<ApprovalRequestDTO>(t)).ToListAsync();
            return Ok(approvalRequests);
        }
        [HttpPatch("approve")]
        [Authorize]
        public async Task<IActionResult> ApproveRequest(ApprovalRequestChangeDTO dto)
        {
            var emp = await EmployeeFunctions.GetUser(manager, User);
            if (emp== null) return Unauthorized();
            var request = await _context.ApprovalRequests.Include(t => t.LeaveRequest).ThenInclude(t=>t.Employee).FirstOrDefaultAsync(t=>t.Id== dto.Id);
            if (request==null) return NotFound();
            var days = request.LeaveRequest.EndDate.DayNumber - request.LeaveRequest.StartDate.DayNumber + 1;
            if(days<= request.LeaveRequest.Employee.OutOfOfficeBalance)
            {
                request.LeaveRequest.Employee.OutOfOfficeBalance -= days;
                request.LeaveRequest.RequestStatus = RequestStatus.Approved;
                request.RequestStatus = RequestStatus.Approved;
                request.Approver = emp;
                request.ApproverId = emp.Id;
                request.Comment = dto.Comment;
                _context.Update(request.LeaveRequest.Employee);
                _context.Update(request);
                await _context.SaveChangesAsync();
                //ApprovalRequestResponseDTO response = new ApprovalRequestResponseDTO() { Approver = mapper.Map<EmployeeDTO>(emp), comment = dto.Comment }; ;
                return Ok();
            }
            return BadRequest(new Error("Not enought on employee balance"));
        }
        [HttpPatch("reject")]
        [Authorize]
        public async Task<IActionResult> RejectRequest(ApprovalRequestChangeDTO dto)
        {
            var emp = await EmployeeFunctions.GetUser(manager, User);
            if (emp == null) return Unauthorized();
            var request = await _context.ApprovalRequests.Include(t => t.LeaveRequest).FirstOrDefaultAsync(t => t.Id == dto.Id);
            if (request == null) return NotFound();
            request.LeaveRequest.RequestStatus = RequestStatus.Rejected;
            request.RequestStatus = RequestStatus.Rejected;
            request.Approver = emp;
            request.ApproverId = emp.Id;
            request.Comment = dto.Comment;
            _context.Update(request);
            await _context.SaveChangesAsync();
            return Ok();
        }
        // GET: api/ApprovalRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApprovalRequest>> GetApprovalRequest(int id)
        {
            var approvalRequest = await _context.ApprovalRequests.FindAsync(id);

            if (approvalRequest == null)
            {
                return NotFound();
            }

            return approvalRequest;
        }

        private bool ApprovalRequestExists(int id)
        {
            return _context.ApprovalRequests.Any(e => e.Id == id);
        }
    }
}
