using Out_of_Office_API.Data;

namespace Out_of_Office_API.DTOs
{
    public class LeaveRequestCreateDTO
    {
        public AbsenceReason AbsenceReason { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Comment { get; set; } = default!;
    }
}
