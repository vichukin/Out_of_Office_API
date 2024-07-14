using Out_of_Office_API.Data;

namespace Out_of_Office_API.DTOs
{
    public class EmployeeRegisterDTO
    {
        public string Password { get; set; } = default!;
        public string Fullname { get; set; } = default!;
        public IFormFile? Photo { get; set; }
        public Position Position { get; set; }
        public SubDivision SubDivision { get; set; }
        public string PeoplePartnerId { get; set; } = default!;
        public int OutOfOfficeBalance {  get; set; }
        public string? Username { get; set; }
        public string? PhotoPath { get; set; }

    }
}
