using BoonBot;
using BoonBot.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
builder.Services.AddSingleton<MessageHandlingService>();

var host = builder.Build();
host.Run();

var client = host.Services.GetRequiredService<DiscordSocketClient>();
var config = host.Services.GetRequiredService<IConfiguration>();
await client.LoginAsync(TokenType.Bot, config["DiscordToken"]);
await client.StartAsync();

var messageHandlingService = host.Services.GetRequiredService<MessageHandlingService>();
await messageHandlingService.InitializeAsync();