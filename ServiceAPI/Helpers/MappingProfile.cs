using AutoMapper;
using Millien.Domain.Entities;
using ServiceAPI.Models.Responses;

namespace ServiceAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Notification, NotificationResponse>();
            CreateMap<NotificationResponse, Notification>();
            CreateMap<Message, MessageReponse>();
            CreateMap<MessageReponse, Message>();
        }
    }
}
