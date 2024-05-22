
using BcService.Abstraction;
using BcService.Abstraction.Handler;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace BcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHostedService<BcS>();

            builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();

            builder.Services.AddSingleton(new TelegramBotClient("6870782628:AAEH_4ldeBRsAiqg67mcGgEoNNH63iDdppw"));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
