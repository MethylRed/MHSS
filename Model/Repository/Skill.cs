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
        public string Name { get; } = string.Empty;

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

        public Skill(string name, int level)
        {
            Name = name;
            Level = level;
        }

    }
}
