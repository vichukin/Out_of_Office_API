using Out_of_Office_API.Data;

namespace Out_of_Office_API.DTOs
{
    public class ProjectCreateDTO
    {
        public ProjectType ProjectType { get; set; }
        public DateOnly StartDate { get; set; }
        public string ProjectManagerId { get; set; } = default!;
        public List<string>? MemberIds { get; set; }
        public string? Comment { get; set; }
    }
}
