using AutoMapper;
using Millien.Domain.Entities;
using Millien.Domain.UnitOfWork.Interfaces;
using ServiceAPI.Models.Responses;
using ServiceAPI.Services.Interfaces;

namespace ServiceAPI.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CountOfUnreadingMessages(int userId)
        {
            var messages = await _unitOfWork.Messages.FindRange(m => m.RecipientId == userId && m.IsRead == false);

            return messages.Count();
        }

        public async Task<List<MessageReponse>> FindCorrespondence(string query, int id)
        {
            var messages = await _unitOfWork.Messages.FindRange(x => x.Text.ToLower().Contains(query.ToLower()) &&
                                                                      (x.SenderId == id || x.RecipientId == id));

            List<MessageReponse> result = new List<MessageReponse>();

            foreach (var message in messages)
            {
                var correspondent = await _unitOfWork.Customers.Find(x => x.Id == message.SenderId);

                result.Add(new MessageReponse(correspondent, message.Text, message.DateOfDispatch.ToLocalTime(), message.IsRead));
            }

            return result;
        }

        public async Task<List<MessageReponse>> GetAllCorresponences(int id)
        {
            var messages = await _unitOfWork.Messages.FindRange(c => c.SenderId == id || c.RecipientId == id);
            var userIds = messages.SelectMany(m => new[] { m.SenderId, m.RecipientId }).Distinct().Where(uid => uid != id);

            List<MessageReponse> result = new List<MessageReponse>();

            foreach (var userId in userIds)
            {
                var correspondent = await _unitOfWork.Customers.Find(x => x.Id == userId);
                var lastMessage = messages.Where(m => (m.SenderId == id && m.RecipientId == userId) || (m.SenderId == userId && m.RecipientId == id)).OrderByDescending(m => m.Id).FirstOrDefault();

                if (lastMessage != null)
                {
                    if (lastMessage.RecipientId == id)
                        result.Add(new MessageReponse(correspondent, lastMessage.Text, lastMessage.DateOfDispatch.ToLocalTime(), lastMessage.IsRead));
                    else
                        result.Add(new MessageReponse(correspondent, lastMessage.Text, lastMessage.DateOfDispatch.ToLocalTime(), true));
                }
            }
            result = result.OrderByDescending(r => r.DateOfDispatch).ToList();

            return result;
        }

        public async Task<List<Message>> GetCorrespondence(int senderId, int recipientId)
        {
            var chat = await _unitOfWork.Messages.FindRange(m => (m.SenderId == senderId && m.RecipientId == recipientId) ||
            (m.SenderId == recipientId && m.RecipientId == senderId));

            foreach (var message in chat)
            {
                message.DateOfDispatch = message.DateOfDispatch.ToLocalTime();
                if (!message.IsRead && message.RecipientId == senderId)
                    message.IsRead = true;
            }

            chat = chat.OrderByDescending(c => c.Id).ToList();
            await _unitOfWork.Save();
            return chat;
        }
    }
}
