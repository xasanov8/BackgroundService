
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace BcService.Abstraction
{
    public class BcS : BackgroundService
    {
        private readonly TelegramBotClient _botClient;
        private readonly IUpdateHandler _updateHandler;

        public BcS(TelegramBotClient botClient, IUpdateHandler updateHandler)
        {
            _botClient = botClient;
            _updateHandler = updateHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _botClient.StartReceiving
                (
                    updateHandler: _updateHandler.HandleUpdateAsync,
                    pollingErrorHandler: _updateHandler.HandlePollingErrorAsync,
                    receiverOptions: new Telegram.Bot.Polling.ReceiverOptions()
                    {
                        ThrowPendingUpdates = true
                    }
                ); ;
        }
    }
}
