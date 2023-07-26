using AutoMapper;
using Millien.Domain.Entities;
using Milien_backend.Models.Requsets;

namespace Milien_backend.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserRequest, Customer>();
        }
    }
}
