using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ServiceAPI.Hubs
{
    [Authorize]
    public class UserStatusHub : Hub
    {
        private static Dictionary<string, int> onlineUsers = new Dictionary<string, int>();
        private readonly ILogger<UserStatusHub> _logger;

        public UserStatusHub(ILogger<UserStatusHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var connectionId = GetConnectionId();
            var id = Convert.ToInt32(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (id != null)
            {
                foreach (var item in onlineUsers.Where(kvp => kvp.Value == id).ToList())
                {
                    onlineUsers.Remove(item.Key);
                }

                onlineUsers[connectionId] = id;

                _logger.LogInformation($"User {id} connected.");
                await Clients.All.SendAsync("UserStatusChanged", id, true);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = GetConnectionId();
            if (onlineUsers.ContainsKey(connectionId))
            {
                onlineUsers.Remove(connectionId);
                await Clients.All.SendAsync("UserStatusChanged", connectionId, false);
            }
            _logger.LogInformation($"User {connectionId} disconnected.");
            await base.OnDisconnectedAsync(exception);
        }

        public bool IsUserOnline(int id)
        {
            _logger.LogInformation($"User {id} is {onlineUsers.ContainsValue(id)}");
            return onlineUsers.ContainsValue(id);
        }

        private string GetConnectionId() => Context.ConnectionId;
    }
}
