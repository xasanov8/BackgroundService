
using BcService.Models;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace BcService.Abstraction.BackgroundServices
{
    public class ConfigureWebhook(
        IConfiguration configuration,
        ITelegramBotClient botClient) : BackgroundService
    {
        private readonly BotConfiguration _configuration = configuration.GetSection("BotConfiguration").Get<BotConfiguration>()!;
        private readonly ITelegramBotClient _botClient = botClient;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var webhookAddress = $"{_configuration.HostAddress}/bot/{_configuration.Token}";

            await _botClient.SendTextMessageAsync(
                chatId: _configuration.MyChatId,
                text: "Start weebhook");

            await _botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: stoppingToken);
        }
    }
}
