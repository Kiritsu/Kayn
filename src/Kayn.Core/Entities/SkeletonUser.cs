using System;
using DSharpPlus.Entities;

namespace Kayn.Core.Entities
{
    public class SkeletonUser
    {
        public ulong Id { get; }
        
        public DateTimeOffset CreatedAt { get; }
        
        public string AvatarUrl { get; }

        public bool IsBot { get; }
        
        public string Username { get; }
        
        public string Discriminator { get; }

        public string FullName => $"{Username}#{Discriminator}";

        public SkeletonUser(DiscordUser user)
        {
            Id = user.Id;
            CreatedAt = user.CreationTimestamp;
            AvatarUrl = user.AvatarUrl;
            Username = user.Username;
            Discriminator = user.Discriminator;
            IsBot = user.IsBot;
        }
    }
}