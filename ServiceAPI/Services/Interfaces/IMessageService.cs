using Millien.Domain.Entities;

namespace ServiceAPI.Services.Interfaces
{
    public interface IMessageService
    {
        Task<List<Message>> GetCorrespondence(int senderId, int recipientId);
        Task<List<Customer>> GetAllCorresponences(int id);
    }
}
