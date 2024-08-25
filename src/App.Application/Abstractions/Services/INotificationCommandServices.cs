using App.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Abstractions.Services
{
    public interface INotificationCommandServices
    {
        Task SendMessageAsync(IEnumerable<MessageInfoDto> messageChatIds);
        void SetChatIds(IEnumerable<string> chatIds);
    }
}
