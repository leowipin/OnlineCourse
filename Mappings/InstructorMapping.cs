using AutoMapper;
using OnlineCourse.Dtos;
using OnlineCourse.Entities;

namespace OnlineCourse.Mappings
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile() 
        {
            CreateMap<InstructorCreationDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<InstructorCreationDto, Instructor>()
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography))
                .ForMember(dest => dest.WebSiteUrl, opt => opt.MapFrom(src => src.WebSiteUrl));
            CreateMap<Instructor, InstructorDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography))
                .ForMember(dest => dest.WebSiteUrl, opt => opt.MapFrom(src => src.WebSiteUrl));
        }
    }
}