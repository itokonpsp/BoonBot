using BoonBot;
using BoonBot.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: false);

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
builder.Services.AddSingleton<MessageHandlingService>();
builder.Services.AddSingleton<InteractionHandlingService>();
builder.Services.AddSingleton<AiChatService>();
builder.Services.AddSingleton<IChatCompletionService>(services =>
{
    var apiKey = services.GetRequiredService<IConfiguration>()["OpenAiApiKey"];
    var model = services.GetRequiredService<IConfiguration>()["OpenAiModel"];
    if(apiKey == null || model == null)
    {
        throw new InvalidOperationException("OpenAI API key or model is not configured.");
    }
    return new OpenAIChatCompletionService(model, apiKey);
});
builder.Services.AddTransient<Kernel>(services => new Kernel(services));

var host = builder.Build();

var client = host.Services.GetRequiredService<DiscordSocketClient>();
var config = host.Services.GetRequiredService<IConfiguration>();
await client.LoginAsync(TokenType.Bot, config["DiscordToken"]);
await client.StartAsync();

var messageHandlingService = host.Services.GetRequiredService<MessageHandlingService>();
await messageHandlingService.InitializeAsync();

var interactionHandlingService = host.Services.GetRequiredService<InteractionHandlingService>();
await interactionHandlingService.InitializeAsync();

host.Run();
