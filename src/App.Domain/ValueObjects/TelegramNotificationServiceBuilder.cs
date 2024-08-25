namespace App.Domain.ValueObjects
{
    public class TelegramNotificationServiceBuilder
    {
        private string _token;
        private string _chatId;

        public TelegramNotificationServiceBuilder SetToken(string token)
        {
            _token = token;
            return this;
        }

        public TelegramNotificationServiceBuilder SetChatId(string chatId)
        {
            _chatId = chatId;
            return this;
        }

        //public ITelegramNotificationService Build()
        //{
        //    if (string.IsNullOrEmpty(_token) || string.IsNullOrEmpty(_chatId))
        //        throw new InvalidOperationException("Token and ChatId must be set.");

        //    //return new TelegramNotificationService(_token, _chatId);
        //}
    }

}
