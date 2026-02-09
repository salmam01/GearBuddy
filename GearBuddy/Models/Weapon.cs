using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
