using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
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
    public class LeaveRequestsController : ControllerBase
    {
        private readonly CompanyDBContext _context;
        private readonly UserManager<Employee> manager;
        private readonly IMapper mapper;

        public LeaveRequestsController(CompanyDBContext context, UserManager<Employee> manager, IMapper mapper)
        {
            _context = context;
            this.manager = manager;
            this.mapper = mapper;
        }

        // GET: api/LeaveRequests
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetLeaveRequests()
        {
            //return await _context.LeaveRequests.ToListAsync();
                var emp = await EmployeeFunctions.GetUser(manager, User);
            if (emp == null) return Unauthorized();
            List<LeaveRequestDTO>? list;
            list = await _context.LeaveRequests.Where(t=>t.EmployeeId==emp.Id).Include(t=>t.Employee).Include(t=>t.ApprovalRequest).Select(t=>mapper.Map<LeaveRequestDTO>(t)).ToListAsync();

            return Ok(list);
        }
        [HttpPost("submitRequest/{leaveRequestId}")]
        [Authorize]
        public async Task<IActionResult> AcceptLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.Include(t => t.Employee).FirstOrDefaultAsync(t=>t.Id== leaveRequestId);
            if(leaveRequest == null) return BadRequest();
            ApprovalRequest approvalRequest = new ApprovalRequest()
            {
                LeaveRequestId = leaveRequestId,
                LeaveRequest = leaveRequest,
                Approver = null,
                ApproverId = null,
                Comment = null,
                RequestStatus= RequestStatus.New

               
            };
            _context.ApprovalRequests.Add(approvalRequest);
            leaveRequest.RequestStatus= RequestStatus.Submitted;
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("cancelRequest/{leaveRequestId}")]
        [Authorize]
        public async Task<IActionResult> CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.Include(t=>t.Employee).FirstOrDefaultAsync(t=>t.Id== leaveRequestId);
            if (leaveRequest == null) return BadRequest();
            leaveRequest.RequestStatus = RequestStatus.Canceled;
            var aprovalRequest = _context.ApprovalRequests.Where(t=>t.LeaveRequestId== leaveRequestId).FirstOrDefault();
            if(aprovalRequest !=null)
            {
                if(aprovalRequest.RequestStatus == RequestStatus.Approved)
                {
                    var days = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber + 1;
                    leaveRequest.Employee.OutOfOfficeBalance += days;
                    _context.Employees.Update(leaveRequest.Employee);
                }
                aprovalRequest.RequestStatus = RequestStatus.Canceled;
                _context.ApprovalRequests.Update(aprovalRequest);
            }
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
            return Ok();
        }
        // GET: api/LeaveRequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest == null)
            {
                return NotFound();
            }
            var leave = mapper.Map<LeaveRequestDTO>(leaveRequest);
            return Ok(leave);
        }

        // PUT: api/LeaveRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveRequest(int id, LeaveRequestCreateDTO leaveRequest)
        {
            var leaveReq = await _context.LeaveRequests.FirstOrDefaultAsync(t=>t.Id== id);
            if (leaveReq == null) return NotFound();
            leaveReq.StartDate = leaveRequest.StartDate;
            leaveReq.EndDate = leaveRequest.EndDate;
            leaveReq.AbsenceReason = leaveRequest.AbsenceReason;
            leaveReq.Comment = leaveRequest.Comment;
            _context.LeaveRequests.Update(leaveReq);
            await _context.SaveChangesAsync();
            return Ok(leaveReq);
        }

        // POST: api/LeaveRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<LeaveRequest>> PostLeaveRequest(LeaveRequestCreateDTO leaveRequest)
        {
            if(ModelState.IsValid)
            {
                var emp = await EmployeeFunctions.GetUser(manager, User);
                if (emp == null) return Unauthorized();
                var lRequest = new LeaveRequest()
                {
                    RequestStatus = RequestStatus.New,
                    EmployeeId = emp.Id,
                    StartDate = leaveRequest.StartDate,
                    EndDate = leaveRequest.EndDate,
                    AbsenceReason = leaveRequest.AbsenceReason,
                    Comment = leaveRequest.Comment,
                    Employee = emp
                };
                
                _context.LeaveRequests.Add(lRequest);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(new Error("Required fields were not specified"));
        }

        // DELETE: api/LeaveRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            _context.LeaveRequests.Remove(leaveRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaveRequestExists(int id)
        {
            return _context.LeaveRequests.Any(e => e.Id == id);
        }
    }
}
