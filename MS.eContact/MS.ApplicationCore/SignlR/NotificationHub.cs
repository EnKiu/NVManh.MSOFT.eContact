using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace MS.eContact.Core
{
    [Authorize]
    public class NotificationHub: Hub, INotificationHub
    {
        readonly IUnitOfWork _unitOfWork;
        public NotificationHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        internal readonly static ConnectionMapping<string> _connections =
           new ConnectionMapping<string>();
        /// <summary>
        /// Kết nối tới Hub
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var userName = Context.User?.Identity?.Name;
            var userId = Context.User?.Claims?.First(x => x.Type == "id").Value;
            Console.WriteLine(userId);
            if (userId != null)
                _connections.Add(userId, Context.ConnectionId);

            //await Clients.Caller.SendAsync("ReceiveNotification", notifications, Context.ConnectionId);
            await Clients.All.SendAsync("ShowAlertWhenOnline", userName);
        }

        /// <summary>
        /// Ngắt kết nối tới Hub
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userName = Context.User?.Identity?.Name;
            if (userName != null)
            {
                _connections.Remove(userName, Context.ConnectionId);
            }

            await Clients.All.SendAsync("ReceiveNotificationWhenDisconnected", Context.User?.Identity?.Name);
        }

        public async Task RemoveConnection()
        {
            var userName = Context.User?.Identity?.Name;
            _connections.Remove(userName, Context.ConnectionId);
        }
        /// <summary>
        /// Gửi tin nhắn tới tất cả mọi người
        /// </summary>
        /// <param name="userName">Tên userName gửi tin nhắnn</param>
        /// <param name="message">Nội dung tin nhắn</param>
        /// <returns></returns>
        /// CreatedBy: NVMANH (19/09/2022)
        public async Task SendMessage(string userName, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }

        /// <summary>
        /// Gửi tin nhắn tới một người cụ thể
        /// </summary>
        /// <param name="userNameTo">Tên người dùng sẽ nhận tin nhắn</param>
        /// <param name="message">Nội dung tin nhắn</param>
        /// <returns></returns>
        /// CreatedBy: NVMANH(14/09/2022)
        public async Task SendMessageToUser(string userNameTo, string message)
        {
            var userNameFrom = Context.User?.Identity?.Name;

            foreach (var connectionId in _connections.GetConnections(userNameTo))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", userNameFrom, message);
            }
        }
        public async Task SendMessageToCaller(string user, string message)
            => await Clients.Caller.SendAsync("ReceiveMessage", user, message);
    }
}
