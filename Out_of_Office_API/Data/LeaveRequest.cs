namespace Out_of_Office_API.Data
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; } = new Employee();
        public AbsenceReason AbsenceReason { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Comment { get; set; } = default!;
        public RequestStatus RequestStatus { get; set; } = RequestStatus.New;

        public ApprovalRequest? ApprovalRequest { get; set; }



    }
}
