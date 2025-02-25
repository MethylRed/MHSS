using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    /// <summary>
    /// 検索条件のクラス
    /// </summary>
    public class Condition
    {
        public List<Equip> Equips = new();

        /// <summary>
        /// スキルの条件
        /// </summary>
        public List<Skill> Skills { get; set; } = new();

        /// <summary>
        /// 最低防御力
        /// </summary>
        public int Def { get; set; }

        /// <summary>
        /// 最低火耐性
        /// </summary>
        public double ResFire { get; set; }

        /// <summary>
        /// 最低水耐性
        /// </summary>
        public double ResWater { get; set; }

        /// <summary>
        /// 最低雷耐性
        /// </summary>
        public double ResThunder { get; set; }
        /// <summary>
        /// 最低氷耐性
        /// </summary>
        public double ResIce { get; set; }

        /// <summary>
        /// 最低龍耐性
        /// </summary>
        public double ResDragon { get; set; }

        /// <summary>
        /// 極意判定用
        /// </summary>
        public bool SatisfySecret { get; set; } = true;

        public Condition() { }


        /// <summary>
        /// 追加スキル用コピーコンストラクタ
        /// </summary>
        /// <param name="other"></param>
        public Condition(Condition other)
        {
            Equips = new();
            foreach (var e in other.Equips)
            {
                Equips.Add(e);
            }
            Skills = new List<Skill>();
            foreach (Skill skill in other.Skills)
            {
                Skills.Add(skill);
            }
            Def = other.Def;
            ResFire = other.ResFire;
            ResWater = other.ResWater;
            ResThunder = other.ResThunder;
            ResIce = other.ResIce;
            ResDragon = other.ResDragon;
            ResDragon = other.ResDragon;
            SatisfySecret = other.SatisfySecret;
        }
    }
}