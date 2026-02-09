using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GearBuddy.Models
{
    public class TuningAffix
    {
        public string Name { get; set; } = string.Empty;
        public List<string> RecommendedFor { get; set; } = [];
    }
}
