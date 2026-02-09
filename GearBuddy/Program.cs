using DSharpPlus;
using DSharpPlus.Commands;
using GearBuddy.Configuration;
using Microsoft.Extensions.Configuration;

class Program
{
    private static DiscordClient? Client { get; set; }
    private static CommandsExtension? Commands { get; set; }

    static async Task Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
            .Build();

        DiscordConfig discordAppConfig = config
            .GetSection("DiscordConfig")
            .Get<DiscordConfig>()
            ?? throw new Exception("Discord Configuration failed.");

        if (string.IsNullOrWhiteSpace(discordAppConfig.AuthToken) ||
            string.IsNullOrWhiteSpace(discordAppConfig.Prefix)
        ) {
            throw new Exception("Discord Configuration failed.");
        }

        // Todo: set up DiscordConfiguration

        // Intents represent what data the bot is allowed to receive from guilds (servers)
        DiscordIntents intents =
            DiscordIntents.AllUnprivileged | 
            DiscordIntents.MessageContents;

        DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(
            discordAppConfig.AuthToken, 
            intents
        );

        // Handle incoming message event & respond to it
        builder.ConfigureEventHandlers(
            b => b.HandleMessageCreated(async (s, e) => 
            {
                // Check if the message starts with the prefix
                if (e.Message.Content.ToLower().StartsWith(discordAppConfig.Prefix)) 
                {
                    await e.Message.RespondAsync("I have no idea what I'm doing. Send help");
                }
            })
        );

        Client = builder.Build();

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }
}

