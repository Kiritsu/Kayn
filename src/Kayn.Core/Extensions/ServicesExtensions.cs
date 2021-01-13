using System;
using DSharpPlus;
using Kayn.Core.Interfaces;
using Kayn.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Kayn.Core.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDiscordService(this IServiceCollection services)
        {
            services.AddSingleton(x =>
            {
                var configuration = x.GetService<IDiscordConfiguration>();
                if (configuration is null)
                {
                    throw new InvalidOperationException(
                        "An implementation of IDiscordConfiguration must be added to dependencies.");
                }

                var logger = x.GetRequiredService<ILoggerFactory>();
                
                return new DiscordClient(new DiscordConfiguration
                {
                    Intents = DiscordIntents.All,
                    LoggerFactory = logger,
                    MessageCacheSize = 4096,
                    MinimumLogLevel = LogLevel.Trace,
                    Token = configuration.Token,
                    TokenType = TokenType.Bot
                });
            });

            services.AddSingleton<ICommandService>(_ => new CommandService(new CommandServiceConfiguration
            {
                IgnoresExtraArguments = true
            }));

            return services.AddHostedService<DiscordService>();
        }
    }
}