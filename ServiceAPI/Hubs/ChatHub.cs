using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Millien.Domain.Entities;
using Millien.Domain.UnitOfWork.Interfaces;
using ServiceAPI.Models.Responses;
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

            if (!string.IsNullOrEmpty(receiver))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, receiver + sender);
                Groups.AddToGroupAsync(Context.ConnectionId, sender + receiver);
            }

            return base.OnConnectedAsync();
        }

        public async Task SendMessageToGroup(string receiver, string message)
        {
            var senderId = Convert.ToInt32(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _unitOfWork.Customers.Find(x => x.Id == senderId);

            Message messageEntity = new Message(senderId, Convert.ToInt32(receiver), message);
            MessageReponse messageResponse = new MessageReponse(user, messageEntity.Text, messageEntity.DateOfDispatch.ToLocalTime(), false);

            await _unitOfWork.Messages.Add(messageEntity);
            await _unitOfWork.Save();
            await Clients.Group(senderId + receiver).SendAsync("ReceiveMessage", messageEntity, messageResponse);
        }
    }
}
