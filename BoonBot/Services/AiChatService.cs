namespace BoonBot.Services;

public class AiChatService
{
    public AiChatService()
    {
    }
    
    public async Task<string> GetResponseAsync(string input)
    {
        // Simulate an AI response
        await Task.Delay(100); // Simulate some processing time
        return $"AI response to: {input}";
    }
}