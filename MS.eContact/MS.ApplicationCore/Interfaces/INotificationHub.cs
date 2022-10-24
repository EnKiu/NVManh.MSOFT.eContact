using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface INotificationHub
    {
        Task SendMessage(string userName, string message);
    }
}
