using DSharpPlus.Entities;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace GearBuddy.Providers
{
    public class CurrencyProvider : IChoiceProvider
    {
        private static readonly IEnumerable<DiscordApplicationCommandOptionChoice> currency =
        [
            new DiscordApplicationCommandOptionChoice("GBP", "GBP"),
            new DiscordApplicationCommandOptionChoice("USD", "USD"),
            new DiscordApplicationCommandOptionChoice("EURO", "EURO"),
        ];

        public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter) =>
            ValueTask.FromResult(currency);
    }
}