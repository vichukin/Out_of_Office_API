namespace Out_of_Office_API.DTOs
{
    public class ApprovalRequestResponseDTO
    {
        public EmployeeDTO Approver {  get; set; }
        public string? comment { get; set; }
    }
}
