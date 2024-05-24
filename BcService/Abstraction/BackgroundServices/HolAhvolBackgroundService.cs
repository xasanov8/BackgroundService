using BcService.Abstraction.Repositories;
using BcService.Models;
using Telegram.Bot;

namespace BcService.Abstraction.BackgroundServices
{
    public class HolAhvolBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ITelegramBotClient _client;

        public HolAhvolBackgroundService(IServiceScopeFactory serviceScopeFactory, ITelegramBotClient client)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var users = await userRepository.GetAllUsers();

                    foreach (var user in users)
                    {
                        await SendNotification(user, stoppingToken);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private Task SendNotification(UserModel user, CancellationToken token)
        {
            try
            {
                return _client.SendTextMessageAsync(
                    chatId: user.Id,
                    text: "Yaxshimisiz aka? Bugun dammi yoki Bugun danmi?",
                    cancellationToken: token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Qaysidir telba blockladi botni");
                return Task.CompletedTask;
            }
        }
    }
}
