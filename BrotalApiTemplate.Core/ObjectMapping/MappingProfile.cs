using AutoMapper;
using BrotalApiTemplate.Domain.DTOs;
using BrotalApiTemplate.Domain.Entities;

namespace BrotalApiTemplate.Core.ObjectMapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
    }
}