namespace Out_of_Office_API.Data
{
    public class Project
    {
        public int Id { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string ProjectManagerId { get; set; } = default!;
        public Employee ProjectManager { get; set; } = default!;
        public List<Employee>? Members { get; set; }
        public string? Comment { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
    }
}
