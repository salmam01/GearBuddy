using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Entities;
using System.ComponentModel;
using System.Threading.Tasks;
using GearBuddy.Services;

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

[Command("pricing")]
public class PricingCommands
{
    [Command("draw")]
    [Description("Calculates the cost of a number of draws.")]
    public static async ValueTask DrawAsync(
        CommandContext context,
        [Description("The number of draws to calculate the cost for.")] int draws,
        [SlashChoiceProvider<CurrencyProvider>] string currency
    )
    {

        int beadQuantity = Pricing.GetBeadQuantityFor(draws);
        List<int> packages = Pricing.GetPackages(beadQuantity);
        string cost = Pricing.GetDrawPrice(packages, currency);
        string packageListStr = Pricing.FormatPackages(packages);

        DiscordInteractionResponseBuilder messageBuilder = new DiscordInteractionResponseBuilder
        {
            Content = $"{draws} requires {beadQuantity} echo beads and costs {cost}! \r\n Packages: \r\n```markdown\r\n{packageListStr}```"
        };
        messageBuilder.AsEphemeral(true);
    
        await context.RespondAsync(messageBuilder);
    }

    [Command("beads")]
    [Description("Calcultes the cost of x beads")]
    public static async ValueTask BeadsAsync(
        CommandContext context,
        [Description("Number of beads to calculate the cost for.")] int beads,
        [SlashChoiceProvider<CurrencyProvider>] string currency
    )
    {
        int beadQuantity = beads;
        List<int> packages = Pricing.GetPackages(beadQuantity);
        double cost = Pricing.PackagesToPrice(packages, currency);
        string result = Pricing.FormatForCurrency(cost, currency);
        string packageListStr = Pricing.FormatPackages(packages);

        DiscordInteractionResponseBuilder messageBuilder = new DiscordInteractionResponseBuilder
        {
            Content = $"{beadQuantity} echo beads and costs {result}! \r\n Packages: \r\n```markdown\r\n{packageListStr}```"
        };
        messageBuilder.AsEphemeral(true);
        
        await context.RespondAsync(messageBuilder);
    }
}