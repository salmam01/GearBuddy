namespace GearBuddy.Models
{
    public class DrawCost
    {
        public Dictionary<string, string> currencyFormat { get; set; } = [];
        public Dictionary<string, Dictionary<string, double>> price { get; set; } = [];
        public List<int> beadPackages { get; set; } = [];
        public int beadsPerDraw { get; set; } = 0;
    }
}