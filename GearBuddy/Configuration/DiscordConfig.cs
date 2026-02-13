namespace GearBuddy.Configuration
{
    public class DiscordConfig
    {
        public string AuthToken { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty;
        public ulong DebugGuildID { get; set; } = 0;
    }
}
