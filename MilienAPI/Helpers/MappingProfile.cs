using AutoMapper;
using MilienAPI.Models;
using MilienAPI.Models.Responses;

namespace MilienAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Customer, AccountResponse>();
        }
    }
}
