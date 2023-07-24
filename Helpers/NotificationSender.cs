using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class NotificationSender : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.Client()
        }
    }
}
