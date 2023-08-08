using Millien.Domain.Entities;
using ServiceAPI.Models.Responses;

namespace ServiceAPI.Services.Interfaces
{
    public interface IMessageService
    {
        Task<List<Message>> GetCorrespondence(int senderId, int recipientId);
        Task<List<MessageReponse>> GetAllCorresponences(int id);
        Task<List<MessageReponse>> FindCorrespondence(string quey, int id);
        Task<int> CountOfUnreadingMessages(int userId);
    }
}
