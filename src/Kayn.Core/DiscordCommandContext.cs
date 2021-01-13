using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Qmmands;

namespace Kayn.Core
{
    public sealed class DiscordCommandContext : CommandContext
    {
        public DiscordClient Client { get; }
        public DiscordGuild Guild { get; }
        public DiscordChannel Channel { get; }
        public DiscordMember Bot { get; }
        public DiscordMember Member { get; }
        public DiscordUser User { get; }
        public DiscordMessage Message { get; }
        
        public DiscordCommandContext(DiscordClient c, MessageCreateEventArgs e, IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            Client = c;
            Guild = e.Guild;
            Channel = e.Channel;
            Bot = e.Guild?.CurrentMember;
            Member = e.Author as DiscordMember;
            User = e.Author;
            Message = e.Message;
        }
        
        public DiscordCommandContext(DiscordClient c, MessageUpdateEventArgs e, IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            Client = c;
            Guild = e.Guild;
            Channel = e.Channel;
            Bot = e.Guild?.CurrentMember;
            Member = e.Author as DiscordMember;
            User = e.Author;
            Message = e.Message;
        }
    }
}