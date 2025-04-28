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
builder.Services.AddSingleton<InteractionHandlingService>();

builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: false);

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
