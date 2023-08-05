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

        public override Task OnConnectedAsync()
        {
            HttpContext httpContext = Context.GetHttpContext();

            string receiver = httpContext.Request.Query["userid"];
            string sender = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Groups.AddToGroupAsync(Context.ConnectionId, sender);
            if (!string.IsNullOrEmpty(receiver))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, receiver);
            }

            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message, int id)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            var senderId = Convert.ToInt32(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));

            Message messageEntity = new Message(senderId, Convert.ToInt32(id), message);

            await _unitOfWork.Messages.Add(messageEntity);
            await _unitOfWork.Save();
        }

        public async Task SendMessageToGroup(string receiver, string message)
        {
            var senderId = Convert.ToInt32(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));

            Message messageEntity = new Message(senderId, Convert.ToInt32(receiver), message);

            await _unitOfWork.Messages.Add(messageEntity);
            await _unitOfWork.Save();
            await Clients.Group(receiver).SendAsync("ReceiveMessage", messageEntity);
        }
    }
}
