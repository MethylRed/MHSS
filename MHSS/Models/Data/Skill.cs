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
        /// スキルのカテゴリ
        /// </summary>
        public string Category { get; set; } = string.Empty;

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

        /// <summary>
        /// 検索条件:スキルレベルの固定
        /// </summary>
        public bool IsFixed { get; set; } = false;

    }
}
