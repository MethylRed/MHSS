using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.Models.Data
{
    public record Skill
    {
        /// <summary>
        /// スキル名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// スキルタイプ
        /// 0:武器スキル
        /// 1:防具スキル
        /// 2:シリーズスキル
        /// 3:グループスキル
        /// 4:その他
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// スキルレベル
        /// </summary>
        public int Level { get; set; } = 0;

        /// <summary>
        /// 普通のスキル：スキルの上限値(極意なし)
        /// シリーズスキル：発動に必要なシリーズスキルの数(少ない方)
        /// </summary>
        public int MaxLevel1 { get; set; } = 0;

        /// <summary>
        /// 普通のスキル：スキルの上限値(極意あり)
        /// シリーズスキル：発動に必要なシリーズスキルの数(多い方)
        /// </summary>
        public int MaxLevel2 { get; set; } = 0;

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
