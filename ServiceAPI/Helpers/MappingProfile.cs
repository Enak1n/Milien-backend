using AutoMapper;
using ServiceAPI.Models;
using ServiceAPI.Models.Responses;

namespace ServiceAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Notification, NotificationResponse>();
            CreateMap<NotificationResponse, Notification>();
        }
    }
}
