using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    public class Master
    {
        public static List<Skill> Skills { get; set; } = new();

        public static List<Equip> AllEquips { get; set; } = new();

        public static List<List<Weapon>> Weapons { get; set; } = new();

        public static List<Equip> Heads { get; set; } = new();

        public static List<Equip> Bodies { get; set; } = new();

        public static List<Equip> Arms { get; set; } = new();

        public static List<Equip> Waists { get; set; } = new();

        public static List<Equip> Legs { get; set; } = new();

        public static List<Equip> Charms { get; set; } = new();

        public static List<Deco> Decos { get; set; } = new();
    }
}
