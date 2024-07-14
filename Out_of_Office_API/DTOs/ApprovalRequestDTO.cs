using Out_of_Office_API.Data;

namespace Out_of_Office_API.DTOs
{
    public class ApprovalRequestDTO
    {
        public int Id { get; set; }
        public EmployeeInfoDTO? Approver { get; set; }
        public LeaveRequestDTO? LeaveRequest { get; set; } 
        public string? Comment { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}
