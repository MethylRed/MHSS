using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    public class Condition
    {
        /// <summary>
        /// スキルの条件
        /// </summary>
        public static List<Skill> Skill { get; set; } = new();

        

        /// <summary>
        /// 最低防御力
        /// </summary>
        public static int Def { get; set; }

        /// <summary>
        /// 最低火耐性
        /// </summary>
        public static int ResFire { get; set; }

        /// <summary>
        /// 最低水耐性
        /// </summary>
        public static int ResWater { get; set; }

        /// <summary>
        /// 最低雷耐性
        /// </summary>
        public static int ResThunder { get; set; }
        /// <summary>
        /// 最低氷耐性
        /// </summary>
        public static int ResIce { get; set; }

        /// <summary>
        /// 最低龍耐性
        /// </summary>
        public static int ResDragon { get; set; }
    }
}