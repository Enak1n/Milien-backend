using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Millien.Domain.Entities;
using Millien.Domain.UnitOfWork.Interfaces;
using System.Security.Claims;

namespace ServiceAPI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SendMessage(string id, string message)
        {
            await Clients.User(id).SendAsync("SendMessage",message);
            var senderId = Convert.ToInt32(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));

            Message messageEntity = new Message(senderId, Convert.ToInt32(id), message);

            await _unitOfWork.Messages.Add(messageEntity);
            await _unitOfWork.Save();
        }
    }
}
