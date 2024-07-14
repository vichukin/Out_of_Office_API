using Out_of_Office_API.Data;

namespace Out_of_Office_API.DTOs
{
    public class LeaveRequestDTO
    {
        public int Id { get; set; }
        public EmployeeInfoDTO Employee { get; set; }
        public AbsenceReason AbsenceReason { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Comment { get; set; } = default!;
        public RequestStatus RequestStatus { get; set; }
        public ApprovalRequestSecondDTO? ApprovalRequest { get; set; }
    }
}
