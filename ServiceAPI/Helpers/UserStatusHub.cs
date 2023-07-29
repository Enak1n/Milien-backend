using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ServiceAPI.Helpers
{
    [Authorize]
    public class UserStatusHub : Hub
    {
        private static Dictionary<string, int> onlineUsers = new Dictionary<string, int>();

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
            await base.OnDisconnectedAsync(exception);
        }

        public bool IsUserOnline(int id)
        {
            if (onlineUsers.ContainsValue(id))
                return true;
            return false;
        }

        private string GetConnectionId() => Context.ConnectionId;
    }
}
