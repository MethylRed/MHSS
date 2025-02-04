using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class Master
    {
        public static List<Skill> Skills { get; set; } = new();

        public static List<Armor> Head { get; set; } = new();
    }
}
