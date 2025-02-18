using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHSS.ViewModels.Controls
{
    internal class SkillLevelSelectorItems : SubViewModelBase
    {
        /// <summary>
        /// ComboBoxのItemに表示させる文字列
        /// </summary>
        public string DisplayStr { get; set; }

        /// <summary>
        /// スキルレベル
        /// </summary>
        public int SkillLevel { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="displayStr">表示用文字列</param>
        /// <param name="skillLevel">スキルレベル</param>
        /// <param name="isFixed">スキルレベル固定の有無</param>
        public SkillLevelSelectorItems(string displayStr, int skillLevel)
        {
            DisplayStr = displayStr;
            SkillLevel = skillLevel;
        }
    }
}
