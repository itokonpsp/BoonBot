using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace BoonBot.Services;

public class AiChatService
{
    private readonly Kernel _kernel;
    
    public AiChatService(Kernel kernel)
    {
        _kernel = kernel;
    }
    
    public async Task<string> GetResponseAsync(string input)
    {
        var chatCompletion = _kernel.GetRequiredService<IChatCompletionService>();
        var response = await chatCompletion.GetChatMessageContentAsync(input, kernel: _kernel);
        return response.Content ?? "";
    }
}