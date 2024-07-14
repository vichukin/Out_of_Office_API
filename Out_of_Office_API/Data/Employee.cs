using Microsoft.AspNetCore.Identity;

namespace Out_of_Office_API.Data
{
    public class Employee : IdentityUser
    {
        public override string Id { get; set; } = default!;
        public override string UserName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public Position Position { get; set; }
        public SubDivision SubDivision { get; set; }
        public bool Status {  get; set; } = true;
        public string? PeoplePartnerId { get; set; }
        public Employee? PeoplePartner {  get; set; }
        public int OutOfOfficeBalance { get; set; }
        public string? PhotoPath { get; set; }

        public List<LeaveRequest>? LeaveRequests { get; set; }
        public List<ApprovalRequest>? Approvals { get; set;}
        public List<Project>? OwnedProjects { get; set; }
        public List<Project>? Projects { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
