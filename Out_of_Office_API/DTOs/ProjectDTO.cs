using Out_of_Office_API.Data;

namespace Out_of_Office_API.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public EmployeeInfoDTO ProjectManager { get; set; } 
        public List<EmployeeInfoDTO>? Members { get; set; }
        public string? Comment { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
    }
}
