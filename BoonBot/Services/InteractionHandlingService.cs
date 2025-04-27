using System.Reflection;
using Discord.Interactions;
using Discord.WebSocket;

namespace BoonBot.Services;

public class InteractionHandlingService
{
    private readonly DiscordSocketClient _discordClient;
    private readonly InteractionService _interaction;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    
    public InteractionHandlingService(DiscordSocketClient discordClient, InteractionService interaction, IServiceProvider services, IConfiguration configuration)
    {
        _discordClient = discordClient;
        _interaction = interaction;
        _services = services;
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        _discordClient.Ready += OnReadyAsync;
        await _interaction.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        _discordClient.InteractionCreated += HandleInteractionAsync;
    }

    private async Task HandleInteractionAsync(SocketInteraction interaction)
    {
        var context = new SocketInteractionContext(_discordClient, interaction);
        await _interaction.ExecuteCommandAsync(context, _services);
    }
    
    private async Task OnReadyAsync()
    {
        if(_configuration["DOTNET_ENVIRONMENT"] == "Development")
        {
            await _interaction.RegisterCommandsToGuildAsync(ulong.Parse(_configuration["GuildId"]));
        }
        else
        {
            await _interaction.RegisterCommandsGloballyAsync();
        }
    }
}
