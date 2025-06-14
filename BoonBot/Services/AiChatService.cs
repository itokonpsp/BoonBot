using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

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
        OpenAIPromptExecutionSettings openAiPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        var response = await chatCompletion.GetChatMessageContentAsync(input, openAiPromptExecutionSettings, _kernel);
        return response.Content ?? "";
    }
}