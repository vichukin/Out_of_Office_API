using Out_of_Office_API.Data;

namespace Out_of_Office_API.DTOs
{
    public class EmployeeDTO
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public Position Position { get; set; }
        public SubDivision SubDivision { get; set; }
        public PeoplePartnerDTO? PeoplePartner { get; set; }
        public int OutOfOfficeBalance { get; set; }
        public string? PhotoPath { get; set; }
        public bool Status { get; set; } = true;

        public List<LeaveRequestDTO>? LeaveRequests { get; set; }
        public List<ProjectDTO>? Projects { get; set; }
        public List<EmployeeInfoDTO>? Employees { get; set; }
    }
}
