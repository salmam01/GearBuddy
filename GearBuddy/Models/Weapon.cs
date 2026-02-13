namespace GearBuddy.Models
{
    // TODO: expand & create JSON file for data, potentially use struct instead of class
    public class Weapon
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Abilities { get; set; } = [];
    }
}
