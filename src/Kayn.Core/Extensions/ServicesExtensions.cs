using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DSharpPlus;
using DSharpPlus.Entities;
using Kayn.Core.Entities;
using Kayn.Core.Interfaces;
using Kayn.Core.Services;
using Kayn.Core.TypeParsers;
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
                    LoggerFactory = logger ,
                    MessageCacheSize = 4096,
                    MinimumLogLevel = LogLevel.Trace,
                    Token = configuration.Token,
                    TokenType = TokenType.Bot
                });
            });

            services.AddSingleton<ICommandService>(x =>
            {
                var commandService = new CommandService(new CommandServiceConfiguration
                {
                    IgnoresExtraArguments = true
                });
                
                // pull or default custom type parsers.
                commandService.AddTypeParser(x.GetService<TypeParser<SkeletonUser>>() ?? SkeletonUserTypeParser.Instance);
                commandService.AddTypeParser(x.GetService<TypeParser<DiscordUser>>() ?? DiscordUserTypeParser.Instance);
                commandService.AddTypeParser(x.GetService<TypeParser<DiscordChannel>>() ?? DiscordChannelTypeParser.Instance);

                return commandService;
            });

            return services.AddHostedService<DiscordService>();
        }
    }
}