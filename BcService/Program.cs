
using BcService.Abstraction;
using BcService.Abstraction.BackgroundServices;
using BcService.Abstraction.Handler;
using BcService.Abstraction.Repositories;
using BcService.Infrostructure;
using BcService.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace BcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<BotUpdateHandler>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Port=16172;Database=BcBotDb;User Id=postgres;Password=axihub;");
            });

            var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

            builder.Services.AddHttpClient("webhook")
                .AddTypedClient<ITelegramBotClient>(httpClient
                    => new TelegramBotClient(botConfig.Token, httpClient));

            builder.Services.AddHostedService<ConfigureWebhook>();
            builder.Services.AddHostedService<HolAhvolBackgroundService>();



            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseCors(ops =>
            {
                ops.AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowAnyOrigin();
            });


            app.UseEndpoints(endpoints =>
            {
                var token = botConfig.Token;

                endpoints.MapControllerRoute(
                    name: "webhook",
                    pattern: $"bot/{token}",
                    new { controller = "WebHookConnect", action = "Connector" });

                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
