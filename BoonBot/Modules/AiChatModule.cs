using Discord.Commands;

namespace BoonBot.Modules;

public class AiChatModule : ModuleBase<SocketCommandContext>
{
    public async Task RespondToMention()
    {
        await ReplyAsync("こんにちわ。メンションされました。");
    }
}
