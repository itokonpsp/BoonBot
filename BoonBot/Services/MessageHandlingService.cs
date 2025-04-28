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
        var userMessage = message as SocketUserMessage;
        if (userMessage == null)
        {
            return;
        }
        var startPos = 0;
        if(userMessage.HasMentionPrefix(_discordClient.CurrentUser, ref startPos) == false)
        {
            return;
        }
        if(userMessage.Author.IsBot)
        {
            return;
        }
        var context = new SocketCommandContext(_discordClient, userMessage);
        var aiChat = _services.GetRequiredService<AiChatService>();
        var response = await aiChat.GetResponseAsync(context.Message.Content[startPos..]);
        await context.Channel.SendMessageAsync(response);
    }
}