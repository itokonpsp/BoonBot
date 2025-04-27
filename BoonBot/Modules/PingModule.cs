using Discord.Commands;
using Discord.Interactions;

namespace BoonBot.Modules;

public class PingModule : InteractionModuleBase<SocketInteractionContext>   
{
    [SlashCommand("ping", "Replies with Pong!")]
    public async Task Ping()
    {
        await RespondAsync("Pong!");
    }
}
