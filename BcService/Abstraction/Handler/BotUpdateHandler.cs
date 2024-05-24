using BcService.Abstraction.Repositories;
using BcService.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BcService.Abstraction.Handler
{
    public class BotUpdateHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ITelegramBotClient botClient;

        public BotUpdateHandler(IServiceScopeFactory serviceScopeFactory, ITelegramBotClient _botClient)
        {
            _serviceScopeFactory = serviceScopeFactory;
            botClient = _botClient;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            var message = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(update.Message, cancellationToken),
                _ => HandleRandomMessageAsync(update.Message, cancellationToken),
            };

            try
            {
                await message;
            }
            catch
            {
                await message;
            }
        }

        private Task HandleRandomMessageAsync(Message? message, CancellationToken cancellationToken)
        {
            Console.WriteLine("{0} sent {1} type message", message?.From.Username, message?.Type);
            return Task.CompletedTask;
        }

        private async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    var user = new UserModel()
                    {
                        Id = message.Chat.Id,
                        Username = message.From.Username
                    };

                    await userRepository.Add(user);

                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"You said:\n<i>{message.Text}</i>",
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
