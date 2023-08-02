using Millien.Domain.Entities;
using Millien.Domain.UnitOfWork.Interfaces;
using ServiceAPI.Services.Interfaces;

namespace ServiceAPI.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Customer>> GetAllCorresponences(int id)
        {
            var customersFromData = await _unitOfWork.Messages.FindRange(c => c.SenderId == id);

            var res = customersFromData.OrderByDescending(m => m.Id).Select(x => new { x.SenderId, x.RecipientId })
                                                                    .Distinct()
                                                                    .ToList();

            List<Customer> result = new List<Customer>();

            foreach (var customer in res)
            {
                var recipient = await _unitOfWork.Customers.Find(x => x.Id == customer.RecipientId);
                result.Add(recipient);
            }

            return result;
        }

        public async Task<List<Message>> GetCorrespondence(int senderId, int recipientId)
        {
            var chat = await _unitOfWork.Messages.FindRange(m => (m.SenderId == senderId && m.RecipientId == recipientId) ||
            (m.SenderId == recipientId && m.RecipientId == senderId));

            return chat.OrderByDescending(c => c.Id).ToList();
        }
    }
}
