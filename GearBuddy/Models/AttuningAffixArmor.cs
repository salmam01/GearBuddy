namespace GearBuddy.Models
{
    public class AttuningAffixArmor
    {
        public string Name { get; set; } = string.Empty;
        public List<string> RecommendedFor { get; set; } = [];
        public List<string> RecommendedForWeapon {  get; set; } = [];
    }
}
