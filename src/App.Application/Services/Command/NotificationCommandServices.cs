using App.Application.Abstractions.Services;
using App.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace App.Application.Services.Command
{
    public class NotificationCommandServices : INotificationCommandServices
    {
        private readonly TelegramBotClient _botClient;
        private IEnumerable<ChatId> _chatIds;

        public NotificationCommandServices(string token)
        {
            _botClient = new TelegramBotClient(token);
        }

        public NotificationCommandServices(string token, string simpleChatId)
        {
            _botClient = new TelegramBotClient(token);
            _chatIds = new List<ChatId> { new ChatId(simpleChatId) };
        }

        public void SetChatIds(IEnumerable<string> chatIds)
        {
            _chatIds = chatIds.Select(id => new ChatId(id));
        }

        public async Task SendMessageAsync(IEnumerable<MessageInfoDto> messageChatIds) //Provider-message-chatId
        {
            if (messageChatIds == null || !messageChatIds.Any())
                throw new InvalidOperationException("ChatIds must be set before sending a message.");

            foreach (var messageInfo in messageChatIds)
            {
                var chatId = new ChatId(messageInfo.ChatId);
                await _botClient.SendTextMessageAsync(chatId, messageInfo.Message);
            }

        }

        public async Task SendSimpleMessageAsync(IEnumerable<MessageInfoDto> messageChatIds) //Provider-message-chatId
        {
            if (_chatIds == null || !_chatIds.Any())
                throw new InvalidOperationException("ChatIds must be set before sending a message.");

            var chatId = _chatIds.FirstOrDefault();
            await _botClient.SendTextMessageAsync(chatId, messageChatIds.FirstOrDefault().Message);
            

        }
    }
}
