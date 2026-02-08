// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.Configuration;


IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
    .Build();

string token = config["AuthToken"] ?? "unknown";

DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(token, DiscordIntents.AllUnprivileged);
DiscordClient client = builder.Build();

await client.ConnectAsync();
// Prevent console window from closing prematurely
await Task.Delay(-1);
