using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.Entities;
using GearBuddy.Commands;
using GearBuddy.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

class Program
{
    static async Task Main(string[] args)
    {
        // Service collection container
        ServiceCollection services = new();

        // Load the app configuration from the JSON files
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
            .Build();

        services.AddSingleton(config);
        
        // Get code after compilation (or something like that idk)
        Assembly currentAssembly = typeof(Program).Assembly;

        try
        {
            DiscordConfig botConfig = config
                .GetSection("DiscordConfig")
                .Get<DiscordConfig>()
                ?? throw new Exception("Discord Bot Configuration failed.");

            if (string.IsNullOrWhiteSpace(botConfig.AuthToken))
            {
                Console.Error.WriteLine("Discord AuthToken is missing.");
                return;
            }

            if (string.IsNullOrWhiteSpace(botConfig.Prefix))
            {
                Console.Error.WriteLine("Bot Prefix is missing.");
                return;
            }

            // Register the Discord Client in the DI container
            services.AddSingleton(serviceProvider =>
            {
                // Intents represent what data the bot is allowed to receive from guilds (servers)
                DiscordIntents intents =
                    DiscordIntents.AllUnprivileged |
                    DiscordIntents.MessageContents |
                    TextCommandProcessor.RequiredIntents |
                    SlashCommandProcessor.RequiredIntents;

                DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(
                    botConfig.AuthToken,
                    intents,
                    services
                );

                // Register the commands extension
                builder.UseCommands(
                    (serviceProvider, extension) =>
                    {
                        // Add Text and Slash Command Processors (input readers) to the extension
                        TextCommandProcessor textCommandProcessor = new(new TextCommandConfiguration()
                        {
                            PrefixResolver = new DefaultPrefixResolver(true, botConfig.Prefix).ResolvePrefixAsync
                        });

                        extension.AddProcessor(textCommandProcessor);
                        extension.AddProcessor(new SlashCommandProcessor());

                        extension.AddCommands(currentAssembly);
                    }, new CommandsConfiguration()
                    {
                        // Server ID for debugging purposes
                        DebugGuildId = botConfig.DebugGuildID
                    }
                );

                // Returns a DiscordClient instance
                return builder.Build();
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            DiscordActivity status = new("Type !goose for help", DiscordActivityType.Playing);

            DiscordClient client = serviceProvider.GetRequiredService<DiscordClient>();

            await client.ConnectAsync();
            await Task.Delay(-1);

        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"An error occurred: {exception}");
            return;
        }
    }
}

