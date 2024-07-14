using Out_of_Office_API.Data;

namespace Out_of_Office_API.DTOs
{
    public class PeoplePartnerDTO
    {
        public string Id { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string? ImagePath { get; set; }
        public string FullName { get; set; } = default!;
        public Position Position { get; set; }
        public SubDivision SubDivision { get; set; }
    }
}
