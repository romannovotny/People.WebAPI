using AutoMapper;
using People.API.Models.DTOs;
using People.Data.Entities;

namespace People.API.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>().ReverseMap();
            CreateMap<Person, PersonPostDto>().ReverseMap();
        }
    }
}