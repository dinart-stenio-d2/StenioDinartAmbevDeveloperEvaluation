using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapping from User to GetUserResult
            // Mapping from User to GetUserResult
            CreateMap<User, GetUserResult>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Username)) // Map `Username` to `Name`
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))   // Map `Email`
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))   // Map `Phone`
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))     // Map `Role`
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) // Map `Status`
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));        // Map `Id`

            // You can add reverse mapping if needed
            CreateMap<GetUserResult, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Name))  // Reverse map `Name` to `Username`
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))    // Map `Email`
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))    // Map `Phone`
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))      // Map `Role`
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))  // Map `Status`
                .ForMember(dest => dest.Id, opt => opt.Ignore())                        // Ignore `Id` if it is auto-generated
                .ForMember(dest => dest.Password, opt => opt.Ignore())                  // Ignore `Password` for security
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())                 // Ignore `CreatedAt` if managed automatically
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Mapping from CreateUserCommand to User
            // Map CreateUserCommand to User
            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username)) // Map Username
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password)) // Map Password
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))       // Map Phone
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))       // Map Email
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))     // Map Status
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))         // Map Role
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) // Set CreatedAt to the current time
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())                   // Ignore UpdatedAt
                .ForMember(dest => dest.Id, opt => opt.Ignore());                         // Ignore Id (usually auto-generated)

            // (Optional) Add reverse mapping if needed
            CreateMap<User, CreateUserCommand>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // Skip password for security
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
        }
    }
}
