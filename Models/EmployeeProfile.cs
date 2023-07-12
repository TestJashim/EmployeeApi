using AutoMapper;
using EmployeeApi.Models.ViewModels;

namespace EmployeeApi.Models
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>()
                .ForMember(e => e.Image, opt => opt.Ignore());

            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
            CreateMap<Employee, Employee>().ReverseMap();


            // Map the Employee entity to the EmployeeDto view model
            CreateMap<Employee, Employee>();

            // Map the EmployeeDto view model to the Employee entity
            CreateMap<EmployeeViewModel, Employee>()
                // Ignore the Id property when mapping from the view model to the entity
                .ForMember(e => e.Id, opt => opt.Ignore());
        }
    }

}
