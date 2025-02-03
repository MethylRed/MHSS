using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public class Armor
    {
        /// <summary>
        /// 防具の名前
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 防具のシリーズ名
        /// </summary>
        public string Kind { get; set; } = string.Empty;

        /// <summary>
        /// スロット1つ目
        /// </summary>
        public int Slot1 { get; set; }

        /// <summary>
        /// スロット2つ目
        /// </summary>
        public int Slot2 { get; set; }

        /// <summary>
        /// スロット3つ目
        /// </summary>
        public int Slot3 { get; set; }

        /// <summary>
        /// 防御力
        /// </summary>
        public int Def { get; set; }

        /// <summary>
        /// 火耐性
        /// </summary>
        public int ResFire { get; set; }

        /// <summary>
        /// 水耐性
        /// </summary>
        public int ResWater { get; set; }

        /// <summary>
        /// 雷耐性
        /// </summary>
        public int ResThunder { get; set; }

        /// <summary>
        /// 氷耐性
        /// </summary>
        public int ResIce { get; set; }

        /// <summary>
        /// 龍耐性
        /// </summary>
        public int ResDragon { get; set; }

        /// <summary>
        /// スキル
        /// </summary>
        
    }
}
