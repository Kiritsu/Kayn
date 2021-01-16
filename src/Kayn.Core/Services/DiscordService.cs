using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Kayn.Core.Extensions;
using Kayn.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Kayn.Core.Services
{
    public sealed class DiscordService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private readonly ICommandService _commands;
        private readonly DiscordClient _client;
        private readonly ILogger<DiscordService> _logger;
        private readonly IDiscordConfiguration _configuration;

        private readonly string[] _prefixes;

        public DiscordService(IServiceProvider services, ICommandService commands, DiscordClient client, 
            ILogger<DiscordService> logger, IDiscordConfiguration configuration)
        {
            _services = services;
            _commands = commands;
            _client = client;
            _logger = logger;
            _configuration = configuration;

            _prefixes = new[]
            {
                _configuration.Prefix,
                $"<@{_configuration.ClientId}>",
                $"<@!{_configuration.ClientId}>"
            };
            
            _commands.AddModules(Assembly.GetExecutingAssembly());
            
            _client.Ready += ClientOnReady;
            _client.MessageCreated += ClientOnMessageCreated;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _client.ConnectAsync(
                new DiscordActivity(_configuration.Game, ActivityType.Watching),
                UserStatus.DoNotDisturb).ConfigureAwait(false);
            
            _logger.LogInformation("Connecting to the discord gateway.");
        }
        
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.DisconnectAsync().ConfigureAwait(false);
            
            _logger.LogInformation("Disconnecting from discord gateway.");
        }

        private Task ClientOnReady(DiscordClient sender, ReadyEventArgs e)
        {
            _logger.LogInformation("Discord bot ready.");

            return Task.CompletedTask;
        }
        
        private async Task ClientOnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Author.IsBot)
            {
                return;
            }

            if (!CommandUtilities.HasAnyPrefix(e.Message.Content, _prefixes, StringComparison.OrdinalIgnoreCase, 
                out var prefix, out var input))
            {
                return;
            }

            var context = new DiscordCommandContext(sender, e, prefix, _services);
            var result = await _commands.ExecuteAsync(input, context);
            if (result.IsSuccessful)
            {
                return;
            }

            _logger.LogWarning("Command result doesn't indicate a success." +
                               $"\n\tContext: {context.Guild.Id} {context.Channel.Id} {context.User.Id} {context.Message.Id}" +
                               $"\n\tInput: <{context.User.GetFullName()}>: {e.Message.Content}" +
                               $"\n\tResult: {result}");
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}