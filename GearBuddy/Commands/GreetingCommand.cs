using DSharpPlus.Commands;


namespace GearBuddy.Commands
{
    public class GreetingCommand
    {
        [Command("greeting")]
        public static async ValueTask ExecuteAsync(CommandContext context) =>
            await context.RespondAsync("Hello World!");
    }
}
