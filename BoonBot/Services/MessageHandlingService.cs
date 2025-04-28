using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using BoonBot.Modules;

namespace BoonBot.Services;

public class MessageHandlingService
{
    private readonly DiscordSocketClient _discordClient;
    private readonly IServiceProvider _services;

    public MessageHandlingService(DiscordSocketClient discordClient, IServiceProvider services)
    {
        _discordClient = discordClient;
        _services = services;
    }

    public async Task InitializeAsync()
    {
        _discordClient.MessageReceived += HandleMessageAsync;
    }

    private async Task HandleMessageAsync(SocketMessage message)
    {
        if (message is SocketUserMessage userMessage)
        {
            var context = new SocketCommandContext(_discordClient, userMessage);
            if (userMessage.MentionedUsers.Any(user => user.Id == _discordClient.CurrentUser.Id))
            {
                using var scope = _services.CreateScope();
                var aiChatModule = new AiChatModule();
                aiChatModule.SetContext(context);
                await aiChatModule.RespondToMention();
            }
        }
    }
}
