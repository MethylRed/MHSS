using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    public class Master
    {
        /// <summary>
        /// 全スキル
        /// </summary>
        public static List<Skill> Skills { get; set; } = new();

        /// <summary>
        /// 全武器
        /// </summary>
        public static List<List<Weapon>> Weapons { get; set; } = new();

        /// <summary>
        /// 全頭防具
        /// </summary>
        public static List<Equip> Heads { get; set; } = new();

        /// <summary>
        /// 全胴防具
        /// </summary>
        public static List<Equip> Bodies { get; set; } = new();

        /// <summary>
        /// 全腕防具
        /// </summary>
        public static List<Equip> Arms { get; set; } = new();

        /// <summary>
        /// 全腰防具
        /// </summary>
        public static List<Equip> Waists { get; set; } = new();

        /// <summary>
        /// 全脚防具
        /// </summary>
        public static List<Equip> Legs { get; set; } = new();

        /// <summary>
        /// 全護石
        /// </summary>
        public static List<Equip> Charms { get; set; } = new();

        /// <summary>
        /// 全装飾品
        /// </summary>
        public static List<Deco> Decos { get; set; } = new();

        /// <summary>
        /// 全装備
        /// </summary>
        public static List<Equip> AllEquips => Heads.Union(Bodies).Union(Arms).Union(Waists)
                                                    .Union(Legs).Union(Charms).Union(Decos)
                                                    .Union(Weapons.SelectMany(w => w)).ToList();

        /// <summary>
        /// 武器が持つスキルのみのリスト
        /// </summary>
        //public static List<string> SkillNamesWithWeapon => Weapons.SelectMany(w => w).SelectMany(w => w.Skills).Select(x => x.Name).Distinct().ToList();
        //public static List<Skill> SkillsWithWeapon => Weapons.SelectMany(w => w).SelectMany(w => w.Skills).GroupBy(x => x.Name).Select(g => g.First()).ToList();
    }
}
