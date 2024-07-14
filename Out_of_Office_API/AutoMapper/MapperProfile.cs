using AutoMapper;
using Out_of_Office_API.Data;
using Out_of_Office_API.DTOs;

namespace Out_of_Office_API.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<LeaveRequest, LeaveRequestCreateDTO>();
            CreateMap<Employee,EmployeeInfoDTO>();
            CreateMap<LeaveRequest,LeaveRequestDTO>();
            CreateMap<Project,ProjectCreateDTO>();
            CreateMap<Project,ProjectDTO>();
            CreateMap<ApprovalRequest, ApprovalRequestDTO>();
            CreateMap<ApprovalRequest, ApprovalRequestSecondDTO>();
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<Employee, PeoplePartnerDTO>();
        }
    }
}
