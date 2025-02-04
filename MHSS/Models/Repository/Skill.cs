using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public record Skill
    {
        /// <summary>
        /// スキル名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// スキルレベル
        /// </summary>
        public int Level { get; set; } = 0;

        /// <summary>
        /// スキルの上限値(極意なし)
        /// </summary>
        public int MaxLevel1 { get; set; }

        /// <summary>
        /// スキルの上限値(極意あり)
        /// </summary>
        public int MaxLevel2 { get; set; }

        ///// <summary>
        ///// コンストラクタ
        ///// </summary>
        ///// <param name="name">スキル名</param>
        ///// <param name="level">装備についているスキルのレベル</param>
        ///// <param name="maxLevel1">スキルの上限値(極意なし)</param>
        ///// <param name="maxLevel2">スキルの上限値(極意あり)</param>
        //public Skill(string name, int level = 0, int maxLevel1 = 0, int maxLevel2 = 0)
        //{
        //    Name = name;
        //    Level = level;
        //    MaxLevel1 = maxLevel1;
        //    MaxLevel2 = maxLevel2;
        //}

    }
}
