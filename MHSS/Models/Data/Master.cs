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
        /// 全装備
        /// </summary>
        public static List<Equip> AllEquips { get; set; } = new();

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


        Dictionary<string, Equip> dicHead = Master.Heads.ToDictionary(kvp => kvp.Name, kvp => kvp);
        Dictionary<string, Deco> dicDeco = Master.Decos.ToDictionary(kvp => kvp.Name, kvp => kvp);

        Dictionary<string, Equip> dic => dicHead
            .Concat(dicDeco.Select(kvp => new KeyValuePair<string, Equip>(kvp.Key, kvp.Value)))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}
