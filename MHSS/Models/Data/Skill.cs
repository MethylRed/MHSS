﻿using System;
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
        /// 発動スキル1
        /// </summary>
        public string ActivateSkillName1 { get; set; } = string.Empty;

        /// <summary>
        /// 発動スキル2
        /// </summary>
        public string ActivateSkillName2 { get; set; } = string.Empty;

        /// <summary>
        /// 普通のスキル：スキルの上限値(極意なし)
        /// シリーズ・グループスキル：発動スキル1に必要なレベル
        /// </summary>
        public int MaxLevel1 { get; set; } = 0;

        /// <summary>
        /// 普通のスキル：スキルの上限値(極意あり)
        /// シリーズ・グループスキル：発動スキル2に必要なレベル
        /// </summary>
        public int MaxLevel2 { get; set; } = 0;

        /// <summary>
        /// 検索条件:スキルレベルの固定
        /// </summary>
        public bool IsFixed { get; set; } = false;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Skill() { }


        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="other"></param>
        public Skill(Skill other)
        {
            Name = other.Name;
            Category = other.Category;
            Level = other.Level;
            ActivateSkillName1 = other.ActivateSkillName1;
            ActivateSkillName2 = other.ActivateSkillName2;
            MaxLevel1 = other.MaxLevel1;
            MaxLevel2 = other.MaxLevel2;
            IsFixed = other.IsFixed;
        }
    }
}
