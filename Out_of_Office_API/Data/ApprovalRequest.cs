namespace Out_of_Office_API.Data
{
    public class ApprovalRequest
    {
        public int Id { get; set; }
        public string? ApproverId { get; set; }
        public Employee? Approver { get; set; } 
        public int LeaveRequestId { get; set; }
        public LeaveRequest LeaveRequest { get; set; } = new LeaveRequest();
        public string? Comment { get; set; }
        public RequestStatus RequestStatus { get; set; } = RequestStatus.New;
    }
}
