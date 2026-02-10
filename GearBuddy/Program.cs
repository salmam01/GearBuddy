using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
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
                    DiscordIntents.MessageContents;

                DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(
                    botConfig.AuthToken,
                    intents,
                    services
                );

                // Register the commands extension
                builder.UseCommands(
                    (serviceProvider, commands) =>
                    {
                        // Processors are input readers
                        commands.AddProcessor(new TextCommandProcessor());
                        commands.AddProcessor(new SlashCommandProcessor());
                        commands.AddCommands(currentAssembly);
                    }
                );

                // Returns a DiscordClient instance
                return builder.Build();
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

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

