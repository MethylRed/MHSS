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
        public List<Skill> Skills { get; set; } = new();

        /// <summary>
        /// 最低防御力
        /// </summary>
        public int Def { get; set; }

        /// <summary>
        /// 最低火耐性
        /// </summary>
        public int ResFire { get; set; }

        /// <summary>
        /// 最低水耐性
        /// </summary>
        public int ResWater { get; set; }

        /// <summary>
        /// 最低雷耐性
        /// </summary>
        public int ResThunder { get; set; }
        /// <summary>
        /// 最低氷耐性
        /// </summary>
        public int ResIce { get; set; }

        /// <summary>
        /// 最低龍耐性
        /// </summary>
        public int ResDragon { get; set; }
    }
}